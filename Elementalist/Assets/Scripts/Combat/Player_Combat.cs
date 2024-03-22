using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    private int multiplier;
    
    public CombatHUD hud;

    public Combat_Manager manager;
    
    public enum State { Idle, Ready, Attack, Heal, ElementalAttack, Dead }

    public State state;
    
    private bool dead = false;
    public bool Dead => dead;

    private GameObject[] enemies;

    private int damage;
    public int Damage => damage;
    
    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;

        max_health = GameManager.Instance.Max_Health;
        
        health_points = GameManager.Instance.HP;
        magic_points = GameManager.Instance.MP;
        attack_power = GameManager.Instance.Attack_Power;
        defence_power = GameManager.Instance.Defence_Power;
        
        hud.setPlayerHP(health_points);
        hud.setMP(magic_points);

        enemies = manager.enemies;
        Debug.Log(enemies.Length);
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

        multiplier = (rand <= 8)? 2: 1;
        
        damage = (attack_power/10) * 15 * multiplier - enemies[0].GetComponent<Grunt_Combat>().Defence_Power;
        enemies[0].GetComponent<Grunt_Combat>().TakeDamage(damage);
        string textToDisplay = "Player attacks and deals " + damage + " damage!";
        
        if (multiplier == 2)
        {
            textToDisplay += " Critical Hit!";
        }

        hud.DialogueText.text = textToDisplay;

        state = State.Idle;
        hud.TogglePlayerActions();
        StartCoroutine(EndTurn());
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
        
        multiplier = (rand <= 8)? 2: 1;

        Element element = GameManager.Instance.selectedElement;
        GameObject projectile = Instantiate(element.GetProjectile(),transform.position, Quaternion.identity);

        Vector3 direction = (enemies[0].transform.position - projectile.transform.position).normalized * (element.GetProjectileSpeed() * Time.deltaTime);
        projectile.transform.LookAt(enemies[0].transform.position);
        projectile.GetComponent<Projectile>().SetMoveDirection(direction);
        
        magic_points -= element.GetMagicCost();
        hud.setMP(magic_points);
        
        damage = (attack_power/10) * element.GetDamageValue() * multiplier - enemies[0].GetComponent<Grunt_Combat>().Defence_Power;
        string textToDisplay = "Player attacks with a " + element.GetAttackName() + " and deals " + damage + " damage!";

        if (multiplier == 2)
        {
            textToDisplay += " Critical Hit!";
        }

        hud.DialogueText.text = textToDisplay;
        
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
        Debug.Log("Ending Player Turn");
        manager.ChangeTurn();
    }

    public (int,int) CalculateCombatRewards()
    {
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
        
        return (newHealth, newMagic);
    }
    
    public void onAttackButton()
    {
        if (state != State.Ready)
            return;

        Attack();
    }

    public void onFireballButton()
    {
        Element element = GameManager.Instance.selectedElement;
        if (state != State.Ready || magic_points < element.GetMagicCost())
            return;

        ElementalAttack();
    }

    public void onHealButton()
    {
        if (state != State.Ready || magic_points < 5)
            return;

        Heal();
    }
}
