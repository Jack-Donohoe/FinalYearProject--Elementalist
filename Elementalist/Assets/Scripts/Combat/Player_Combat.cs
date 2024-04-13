using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Combat : MonoBehaviour
{
    private int max_health = 100;
    public int Max_Health
    {
        get => max_health;
        set => max_health = value;
    }
    
    private int health_points = 100;
    public int HP
    {
        get => health_points;
        set => health_points = value;
    }
    
    private int max_magic = 60;
    public int Max_Magic
    {
        get => max_magic;
        set => max_magic = value;
    }
    
    private int magic_points = 60;
    public int MP
    {
        get => magic_points;
        set => magic_points = value;
    }
    
    private int attack_power = 10;
    public int Attack_Power
    {
        get => attack_power;
        set => attack_power = value;
    }
    
    private int defence_power = 5;
    public int Defence_Power
    {
        get => defence_power;
        set => defence_power = value;
    }
    
    private int crit_multiplier;
    
    public CombatHUD hud;

    public Combat_Manager manager;
    
    public enum State { Idle, Ready, Attack, Heal, ElementalAttack, Dead }

    public State state;
    
    private bool dead = false;
    public bool Dead => dead;

    private GameObject[] enemies;

    private string action;

    private int damage;
    public int Damage => damage;

    private string dialogue;

    public Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;

        max_health = GameManager.Instance.Max_Health;
        max_magic = GameManager.Instance.Max_Magic;
        
        health_points = GameManager.Instance.HP;
        magic_points = GameManager.Instance.MP;
        attack_power = GameManager.Instance.Attack_Power;
        defence_power = GameManager.Instance.Defence_Power;
        
        hud.setPlayerHP(health_points);
        hud.setMP(magic_points);

        animator = GetComponent<Animator>();
    }

    public void SetEnemies()
    {
        enemies = manager.spawnedEnemies;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            state = State.Dead;
        }
        
        switch (state)
        {
            case State.Idle:
            {
                break;
            }
            case State.Ready:
            {
                SetEnemies();
                break;
            }
            case State.Attack:
            {
                Attack();
                break;
            }
            case State.Heal:
            {
                Heal();
                break;
            }
            case State.ElementalAttack:
            {
                ElementalAttack();
                break;
            }
        }
    }
    
    private void Attack()
    {
        int rand = Random.Range(0, 100);

        crit_multiplier = (rand <= 5)? 2: 1;
        
        damage = Mathf.RoundToInt(attack_power + Random.Range(5,10) * crit_multiplier - enemies[0].GetComponent<Grunt_Combat>().Defence_Power);
        dialogue = "Player attacks and deals " + damage + " damage!";
        
        if (crit_multiplier == 2)
        {
            dialogue += " Critical Hit!";
        }

        action = "Attack";
    }

    private void Heal()
    {
        int healthToRestore = (Random.value > 0.7f) ? 10 : 5;
        health_points += healthToRestore;
        
        if (health_points > max_health)
        {
            health_points = max_health;
        }
        
        hud.setPlayerHP(health_points);
        
        magic_points -= 5;
        hud.setMP(magic_points);
        hud.DialogueText.text = "Player uses Heal and restores " + healthToRestore + "HP.";
        
        state = State.Idle;
        hud.TogglePlayerActions();
        StartCoroutine(EndTurn());
    }

    private void ElementalAttack()
    {
        int rand = Random.Range(0, 100);
        
        crit_multiplier = (rand <= 5)? 2: 1;

        Element element = GameManager.Instance.selectedElement;
        int damageVal = element.GetDamageValue();
        Grunt_Combat enemyCombat = enemies[0].GetComponent<Grunt_Combat>();

        damage = Mathf.RoundToInt(attack_power + Random.Range(damageVal - 5, damageVal) * crit_multiplier
                                  * ElementManager.Instance.GetDamageMultiplier((element.GetName(), enemyCombat.element.GetName())) - enemyCombat.Defence_Power);
        dialogue = "Player attacks with a " + element.GetAttackName() + " and deals " + damage + " damage!";
        
        magic_points -= element.GetMagicCost();
        hud.setMP(magic_points);

        if (crit_multiplier == 2)
        {
            dialogue += " Critical Hit!";
        }

        action = "Elemental Attack";
    }

    private IEnumerator LaunchProjectile(GameObject attackTarget)
    {
        yield return new WaitForSeconds(0.7f);
        
        Element element = GameManager.Instance.selectedElement;
        GameObject projectileToLaunch = Instantiate(element.GetProjectile(), new Vector3(transform.position.x, 1.5f, transform.position.z), Quaternion.identity);
        
        Vector3 direction = (attackTarget.transform.position - projectileToLaunch.transform.position).normalized * (element.GetProjectileSpeed() * Time.deltaTime);
        projectileToLaunch.transform.LookAt(enemies[0].transform.position);
        projectileToLaunch.GetComponent<Projectile>().SetMoveDirection(direction);
    }

    private void PerformAction(GameObject attackTarget)
    {
        if (action == "Elemental Attack")
        {
            animator.SetBool("Attacking", true);
            StartCoroutine(LaunchProjectile(attackTarget));
        }
        else if (action == "Attack")
        {
            attackTarget.GetComponent<Grunt_Combat>().TakeDamage(damage);
            StartCoroutine(EndTurn());
        }
        
        hud.DialogueText.text = dialogue;
        
        state = State.Idle;
        hud.TogglePlayerActions();
    } 

    public void TakeDamage(int damage)
    {
        health_points -= damage;
        
        if (health_points <= 0)
        {
            health_points = 0;
            dead = true;
        } 
        
        hud.setPlayerHP(health_points);
    }
    
    public void StartTurn()
    {
        state = State.Ready;
        hud.TogglePlayerActions();
    }

    public void StartEndTurn()
    {
        StartCoroutine(EndTurn());
    }
    
    private IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(1f);
        manager.ChangeTurn();
    }

    public (int,int,int) CalculateCombatRewards()
    {
        int xp = enemies.Length * 50;
        
        int newHealth = health_points + max_health / 10;
        if (newHealth > max_health)
        {
            newHealth = max_health;
        }

        int newMagic = magic_points + 15;
        if (newMagic > max_magic)
        {
            newMagic = max_magic;
        }
        
        return (newHealth, newMagic, xp);
    }

    private void ShowTargetOptions()
    {
        hud.RefreshTargetButtons(enemies);
        hud.ActionPanel.SetActive(false);
        hud.TargetPanel.SetActive(true);
        hud.DialogueText.text = "Select Target";
    }
    
    public void onAttackButton()
    {
        if (state != State.Ready)
            return;

        Attack();
        
        ShowTargetOptions();
    }

    public void onElementAttackButton()
    {
        Element element = GameManager.Instance.selectedElement;
        if (state != State.Ready || magic_points < element.GetMagicCost())
        {
            hud.DialogueText.text = "MP Too Low";
            return;
        }

        ElementalAttack();
        
        ShowTargetOptions();
    }

    public void onHealButton()
    {
        if (state != State.Ready || magic_points < 5)
            return;

        Heal();
    }

    public void onTargetButton(Button button)
    {
        Button[] targetButtons = hud.targetButtons;
        
        for (int i = 0; i < targetButtons.Length; i++)
        {
            if (button == targetButtons[i])
            {
                if (!enemies[i].GetComponent<Grunt_Combat>().Dead)
                {
                    PerformAction(enemies[i]);
                }
                else
                {
                    return;
                }
            }
        }
    }
}
