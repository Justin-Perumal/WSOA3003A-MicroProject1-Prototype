using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum BattleStates {START, PLAYERTURN, ENEMYTURN, WON, LOST}

public class BattleController : MonoBehaviour
{

    public GameObject PlayerPrefab;
    public GameObject EnemyPrefab;
    public Transform PlayerPos;
    public Transform EnemyPos;
    public GameObject CommandText;
    public Transform CardPos1;
    public Transform CardPos2;
    public Transform CardPos3;

    public BattleStates states;

    public CardManager CM;
    public GameObject Choice1, Choice2, Choice3;

    Unit PlayerUnit;
    Unit EnemyUnit;

    public HUDControl PlayerHUD;
    public HUDControl EnemyHUD;

    private bool EnemyDead = false;
    private bool PlayerDead = false;

    public bool PlayerIsDefending = false;
    public bool EnemyIsDefending = false;

    public bool EnemyIsCrippled = false;
    public int EnemyCrippleCounter;
    public bool PlayerIsCrippled = false;
    public int PlayerCrippleCounter;

    public bool EnemyIsPoisoned = false;
    public int EnemyPoisonCounter;
    public bool PlayerIsPoisoned = false;
    public int PlayerPoisonCounter;

    public GameObject EnemyCrippleText, PlayerCrippleText, EnemyPoisonText, PlayerPoisonText;

    // Start is called before the first frame update
    void Start()
    {
        states = BattleStates.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        CommandText.GetComponent<TextMeshProUGUI>().text = "Battle has begun";

        GameObject PlayerObj = Instantiate(PlayerPrefab, PlayerPos);
        PlayerUnit = PlayerObj.GetComponent<Unit>();

        GameObject EnemyObj = Instantiate(EnemyPrefab, EnemyPos);
        EnemyUnit = EnemyObj.GetComponent<Unit>();

        PlayerHUD.SetHUD(PlayerUnit);
        EnemyHUD.SetHUD(EnemyUnit);

        yield return new WaitForSeconds(3f);

        states = BattleStates.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        CommandText.GetComponent<TextMeshProUGUI>().text = "Choose a command";

        int Random1 = Random.Range(0,CM.Cards.Length);
        Choice1 = Instantiate(CM.Cards[Random1],CardPos1);

        if (Choice1.CompareTag("Attack"))
        {
            Choice1.GetComponent<Button>().onClick.AddListener(Attack);
        }
        if(Choice1.CompareTag("Defend"))
        {
            Choice1.GetComponent<Button>().onClick.AddListener(Defend);
        }
        if(Choice1.CompareTag("Heal"))
        {
            Choice1.GetComponent<Button>().onClick.AddListener(Heal);
        } 
        if(Choice1.CompareTag("Cripple"))
        {
            Choice1.GetComponent<Button>().onClick.AddListener(Cripple);
        } 
        if(Choice1.CompareTag("Poison"))
        {
            Choice1.GetComponent<Button>().onClick.AddListener(Poison);
        } 
        if(Choice1.CompareTag("Lifesteal"))
        {
            Choice1.GetComponent<Button>().onClick.AddListener(Lifesteal);
        } 

        int Random2 = Random.Range(0,CM.Cards.Length);
        Choice2 = Instantiate(CM.Cards[Random2],CardPos2);
        if (Choice2.CompareTag("Attack"))
        {
            Choice2.GetComponent<Button>().onClick.AddListener(Attack);
        }
        if(Choice2.CompareTag("Defend"))
        {
            Choice2.GetComponent<Button>().onClick.AddListener(Defend);
        }
        if(Choice2.CompareTag("Heal"))
        {
            Choice2.GetComponent<Button>().onClick.AddListener(Heal);
        } 
        if(Choice2.CompareTag("Cripple"))
        {
            Choice2.GetComponent<Button>().onClick.AddListener(Cripple);
        } 
        if(Choice2.CompareTag("Poison"))
        {
            Choice2.GetComponent<Button>().onClick.AddListener(Poison);
        } 
        if(Choice2.CompareTag("Lifesteal"))
        {
            Choice2.GetComponent<Button>().onClick.AddListener(Lifesteal);
        } 

        int Random3 = Random.Range(0,CM.Cards.Length);
        Choice3 = Instantiate(CM.Cards[Random3],CardPos3);
        if (Choice3.CompareTag("Attack"))
        {
            Choice3.GetComponent<Button>().onClick.AddListener(Attack);
        }
        if(Choice3.CompareTag("Defend"))
        {
            Choice3.GetComponent<Button>().onClick.AddListener(Defend);
        }
        if(Choice3.CompareTag("Heal"))
        {
            Choice3.GetComponent<Button>().onClick.AddListener(Heal);
        } 
        if(Choice3.CompareTag("Cripple"))
        {
            Choice3.GetComponent<Button>().onClick.AddListener(Cripple);
        } 
        if(Choice3.CompareTag("Poison"))
        {
            Choice3.GetComponent<Button>().onClick.AddListener(Poison);
        } 
        if(Choice3.CompareTag("Lifesteal"))
        {
            Choice3.GetComponent<Button>().onClick.AddListener(Lifesteal);
        } 
    }

    void DestroyChoice()
    {
        Destroy(Choice1);
        Destroy(Choice2);
        Destroy(Choice3);
    }

    public void Attack()
    {
        if(states != BattleStates.PLAYERTURN)
            return;
        
        StartCoroutine(PlayerAttack());
        DestroyChoice();

        Debug.Log("Attacking");
    }

    public void Defend()
    {
        if(states != BattleStates.PLAYERTURN)
            return;

        StartCoroutine(PlayerDefend());
        DestroyChoice();

        Debug.Log("Defending");
    }

    public void Heal()
    {
        if(states != BattleStates.PLAYERTURN)
            return;

        StartCoroutine(PlayerHeal());
        DestroyChoice();

        Debug.Log("Healing");
    }

    public void Cripple()
    {
        if(states != BattleStates.PLAYERTURN)
            return;

        StartCoroutine(PlayerCripple());
        DestroyChoice();
    }

    public void Poison()
    {
        if(states != BattleStates.PLAYERTURN)
            return;

        StartCoroutine(PlayerPoison());
        DestroyChoice();
    }

    public void Lifesteal()
    {
        if(states != BattleStates.PLAYERTURN)
            return;
        
        StartCoroutine(PlayerLifesteal());
        DestroyChoice();
    }

    IEnumerator PlayerAttack()
    {
        PlayerDmg();
        CommandText.GetComponent<TextMeshProUGUI>().text = "Attacking";

        yield return new WaitForSeconds(2f);

        EnemyHUD.SetHUD(EnemyUnit);

        if(EnemyUnit.CurrentHP <= 0)
        {
            states = BattleStates.WON;
            EnemyDead = true;
            EndBattle();
        }
        else
        {
            states = BattleStates.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerDefend()
    {
        PlayerIsDefending = true;
        CommandText.GetComponent<TextMeshProUGUI>().text = "Defending";

        yield return new WaitForSeconds(2f);

        states = BattleStates.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerHeal()
    {
        CommandText.GetComponent<TextMeshProUGUI>().text = "Healing";

        yield return new WaitForSeconds(2f);

        PlayerUnit.CurrentHP += PlayerUnit.HealAmount;
        states = BattleStates.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerCripple()
    {
        CommandText.GetComponent<TextMeshProUGUI>().text = "Crippling enemy";

        yield return new WaitForSeconds(2f);

        EnemyIsCrippled = true;
        EnemyCrippleCounter = 3;

        EnemyCrippleText.GetComponent<TextMeshProUGUI>().text = "Enemy is crippled";
        states = BattleStates.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerPoison()
    {
        CommandText.GetComponent<TextMeshProUGUI>().text = "Poisoning enemy";

        yield return new WaitForSeconds(2f);

        EnemyIsPoisoned = true;
        EnemyPoisonCounter = 3;

        EnemyPoisonText.GetComponent<TextMeshProUGUI>().text = "Enemy is poisoned";
        states = BattleStates.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerLifesteal()
    {
        CommandText.GetComponent<TextMeshProUGUI>().text = "Lifestealing from enemy";

        yield return new WaitForSeconds(2f);

        PlayerUnit.CurrentHP++;
        EnemyUnit.CurrentHP--;

        EnemyHUD.SetHUD(EnemyUnit);

        if(EnemyUnit.CurrentHP <= 0)
        {
            states = BattleStates.WON;
            EnemyDead = true;
            EndBattle();
        }
        else
        {
            states = BattleStates.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

    }

    IEnumerator EnemyTurn()
    {
        if(EnemyIsPoisoned)
        {
            EnemyUnit.CurrentHP--;
            EnemyHUD.SetHUD(EnemyUnit);

            if(EnemyUnit.CurrentHP <= 0)
            {
                states = BattleStates.WON;
            }

            if(EnemyPoisonCounter > 0)
            {
                EnemyPoisonCounter--;
                if(EnemyPoisonCounter == 0)
                {
                    EnemyIsPoisoned = false;
                    EnemyPoisonText.GetComponent<TextMeshProUGUI>().text = "";
                }
            }
        }

        CommandText.GetComponent<TextMeshProUGUI>().text = "Enemies turn";

        EnemyDmg();

        if(EnemyCrippleCounter == 0)
        {
            EnemyIsCrippled = false;
            EnemyCrippleText.GetComponent<TextMeshProUGUI>().text = "";
        }

        yield return new WaitForSeconds(2f);

        PlayerHUD.SetHUD(PlayerUnit);

        if(PlayerUnit.CurrentHP <= 0)
        {
            states = BattleStates.LOST;
            PlayerDead = true;
            EndBattle();
        }
        else
        {
            states = BattleStates.PLAYERTURN;
            PlayerTurn();
        }
    }

    public void EndBattle()
    {
        if(states == BattleStates.WON)
        {
            Debug.Log("You have won");
        }
        else if(states == BattleStates.LOST)
        {
            Debug.Log("You have lost");
        }
    }

    void PlayerDmg()
    {
        if(EnemyIsDefending)
        {
          EnemyUnit.CurrentHP -= Mathf.RoundToInt(PlayerUnit.Damage/2);  
          EnemyIsDefending = false;
        }
        else
        {
          EnemyUnit.CurrentHP -= PlayerUnit.Damage;
        }
    }

    void EnemyDmg()
    {
        if(EnemyIsCrippled && EnemyCrippleCounter > 0)
        {
            if(PlayerIsDefending)
            {
                PlayerUnit.CurrentHP -= Mathf.RoundToInt((EnemyUnit.Damage-1)/2);
                PlayerIsDefending = false;
            }
            else
            {
                PlayerUnit.CurrentHP -= (EnemyUnit.Damage-1);
            }
            EnemyCrippleCounter--;
        }
        else if(!EnemyIsCrippled)
        {
             if(PlayerIsDefending)
            {
                PlayerUnit.CurrentHP -= Mathf.RoundToInt((EnemyUnit.Damage)/2);
                PlayerIsDefending = false;
            }
            else
            {
                PlayerUnit.CurrentHP -= (EnemyUnit.Damage);
            }
        }
    }

}
