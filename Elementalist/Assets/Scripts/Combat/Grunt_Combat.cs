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
    
    private int crit_multiplier;
    
    public CombatHUD hud;

    public Combat_Manager manager;
    
    public enum State { Idle, Attack, Heal, ElementalAttack, Dead }

    public State state;
    
    private bool dead = false;
    public bool Dead => dead;

    private Player_Combat player;

    public Element element;
    
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<Combat_Manager>();
        
        state = State.Idle;

        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<CombatHUD>();
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

        if (rand < 40 && magic_points >= 5)
        {
            state = State.ElementalAttack;
        }
        else
        {
            state = State.Attack;
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

        crit_multiplier = (rand <= 5)? 2: 1;

        int damage = Mathf.RoundToInt(attack_power + Random.Range(5,10) * crit_multiplier - player.Defence_Power);
        player.TakeDamage(damage);
        hud.DialogueText.text = "Enemy A attacks and deals " + damage + " damage!";
        
        StartCoroutine(EndTurn());
    }

    private void ElementalAttack()
    {
        int rand = Random.Range(0, 100);
        
        crit_multiplier = (rand <= 5)? 2: 1;
        
        int damage = Mathf.RoundToInt(attack_power + Random.Range(15,20) * crit_multiplier - player.Defence_Power);
        player.TakeDamage(damage);
        magic_points -= 5;
        string textToDisplay = "Enemy A uses an Elemental Attack and deals " + damage + " damage!";
        
        if (crit_multiplier == 2)
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
