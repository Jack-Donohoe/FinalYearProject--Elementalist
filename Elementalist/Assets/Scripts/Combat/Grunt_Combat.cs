using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt_Combat : MonoBehaviour
{
    private int max_health = 50;
    public int Max_Health
    {
        get => max_health;
        set => max_health = value;
    }
    
    private int health_points = 50;
    public int HP
    {
        get => health_points;
        set => health_points = value;
    }
    
    private int max_magic = 50;
    public int Max_Magic
    {
        get => max_magic;
        set => max_magic = value;
    }
    
    private int magic_points = 20;
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
    
    public enum State { Idle, Attack, Heal, ElementalAttack, Dead }

    public State state;
    
    private bool dead = false;
    public bool Dead => dead;

    private Player_Combat player;
    
    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
        
        hud.setEnemyHP(health_points);
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Combat>();
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

    public IEnumerator StartTurn()
    {
        yield return new WaitForSeconds(0.5f);
        int rand = Random.Range(0, 100);

        if (rand >= 30)
        {
            state = State.Attack;
        } else if (rand is > 20 and < 30 && magic_points >= 5)
        {
            state = State.Heal;
        }
        else if (rand <= 20 && magic_points >= 5)
        {
            state = State.ElementalAttack;
        }
    }

    private IEnumerator EndTurn()
    {
        state = State.Idle;
        
        yield return new WaitForSeconds(1.5f);
        manager.ChangeTurn();
    }

    private void Attack()
    {
        int rand = Random.Range(0, 100);

        multiplier = (rand <= 8)? 2: 1;

        int damage = (attack_power/10) * 10 * multiplier - player.Defence_Power;
        player.TakeDamage(damage);
        hud.DialogueText.text = "Enemy A attacks and deals " + damage + " damage!";
        
        StartCoroutine(EndTurn());
    }

    private void Heal()
    {
        int healthToRestore = (Random.value > 0.7f) ? 10 : 5;
        health_points += healthToRestore;
        
        if (health_points > 100)
        {
            health_points = 100;
        }
        
        hud.setEnemyHP(health_points);
        magic_points -= 5;
        string textToDisplay = "Enemy A uses Heal and restores " + healthToRestore + "HP";
        
        if (multiplier == 2)
        {
            textToDisplay += " Critical Hit!";
        }

        hud.DialogueText.text = textToDisplay;
        
        StartCoroutine(EndTurn());
    }

    private void ElementalAttack()
    {
        int rand = Random.Range(0, 100);
        
        multiplier = (rand <= 8)? 2: 1;
        
        int damage = (attack_power/10) * 20 * multiplier - player.Defence_Power;
        player.TakeDamage(damage);
        magic_points -= 5;
        string textToDisplay = "Enemy A uses an Elemental Attack and deals " + damage + " damage!";
        
        if (multiplier == 2)
        {
            textToDisplay += " Critical Hit!";
        }

        hud.DialogueText.text = textToDisplay;
        
        StartCoroutine(EndTurn());
    }

    public void TakeDamage(int damage)
    {
        health_points -= damage;
        
        if (health_points <= 0)
        {
            health_points = 0;
            dead = true;
        } 
        
        hud.setEnemyHP(health_points);
    }
}
