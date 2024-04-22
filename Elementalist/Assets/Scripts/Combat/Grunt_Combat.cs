using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grunt_Combat : MonoBehaviour
{
    private int health_points = 50;
    
    private int magic_points = 15;

    private int attack_power = 15;
    
    private int defence_power = 5;
    public int Defence_Power
    {
        get => defence_power;
        set => defence_power = value;
    }
    
    private int crit_multiplier;
    
    public CombatHUD hud;

    public Combat_Manager manager;
    
    private bool dead;
    public bool Dead => dead;

    private Player_Combat player;

    public String enemy_Name = "Grunt";

    public Element element;

    public bool eliteEnemy;
    
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<Combat_Manager>();
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Combat>();

        if (eliteEnemy)
        {
            enemy_Name = "Elite " + enemy_Name;
            health_points = Mathf.RoundToInt(health_points * 2f * GameManager.Instance.levelInt);
            magic_points = Mathf.RoundToInt(magic_points * 0.75f * 2f * GameManager.Instance.levelInt);
            attack_power = Mathf.RoundToInt(attack_power * 0.8f * 2f * GameManager.Instance.levelInt);
            defence_power = Mathf.RoundToInt(defence_power * 2f * GameManager.Instance.levelInt);
        }
        else if(GameManager.Instance.levelInt != 1)
        {
            health_points = Mathf.RoundToInt(health_points * 0.75f  * GameManager.Instance.levelInt);
            magic_points = Mathf.RoundToInt(magic_points * 0.75f * GameManager.Instance.levelInt);
            attack_power = Mathf.RoundToInt(attack_power * 0.75f * GameManager.Instance.levelInt);
            defence_power = Mathf.RoundToInt(defence_power * 0.75f * GameManager.Instance.levelInt);
        }
    }

    public (int, String) SelectAction()
    {
        (int, String) action;
        int rand = Random.Range(0, 100);

        if (rand > 40 || magic_points < 5)
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
        if (damage <= 0)
        {
            damage = 1;
        }
        
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
        if (damage <= 0)
        {
            damage = 1;
        }
        
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
    }
}
