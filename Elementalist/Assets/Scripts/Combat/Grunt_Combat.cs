using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    
    private bool dead = false;
    public bool Dead => dead;

    private Player_Combat player;

    public String enemy_Name = "Grunt";

    public Element element;
    
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<Combat_Manager>();

        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<CombatHUD>();
        hud.setEnemyHP(health_points);
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Combat>();
    }

    public (int, String) SelectAction()
    {
        (int, String) action;
        int rand = Random.Range(0, 100);

        if (rand < 40 && magic_points >= 5)
        {
            action = Attack();
        }
        else
        {
            action = ElementalAttack();
        }

        return action;
    }

    private (int, String) Attack()
    {
        int rand = Random.Range(0, 100);

        crit_multiplier = (rand <= 5)? 2: 1;

        int damage = Mathf.RoundToInt(attack_power + Random.Range(5,10) * crit_multiplier - player.Defence_Power);
        string textToDisplay = enemy_Name +" attacks and deals " + damage + " damage!";
        
        if (crit_multiplier == 2)
        {
            textToDisplay += " Critical Hit!";
        }

        return (damage, textToDisplay);
    }

    private (int, String) ElementalAttack()
    {
        int rand = Random.Range(0, 100);
        
        crit_multiplier = (rand <= 5)? 2: 1;
        
        int damage = Mathf.RoundToInt(attack_power + Random.Range(15,20) * crit_multiplier - player.Defence_Power);
        magic_points -= 5;
        string textToDisplay = enemy_Name + " uses an Elemental Attack and deals " + damage + " damage!";
        
        if (crit_multiplier == 2)
        {
            textToDisplay += " Critical Hit!";
        }
        
        return (damage, textToDisplay);
    }

    public void TakeDamage(int damage)
    {
        health_points -= damage;
        
        if (health_points <= 0)
        {
            health_points = 0;
            dead = true;
            gameObject.SetActive(false);
        } 
        
        hud.setEnemyHP(health_points);
    }
}
