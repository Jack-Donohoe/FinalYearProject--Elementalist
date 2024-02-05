using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt_Combat : MonoBehaviour
{
    private int health_points = 40;
    private int magic_points = 20;
    public int attack_power = 5;
    private int defence_power = 2;
    
    private int multiplier;
    
    public CombatHUD hud;

    public Combat_Manager manager;
    
    public enum State { Idle, Attack, Heal, ElementalAttack }

    public State state;
    
    private bool dead = false;

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

    public void StartTurn()
    {
        Debug.Log("Enemy Turn");
        int rand = Random.Range(0, 100);

        if (rand > 30)
        {
            state = State.Attack;
        } else if (rand is > 20 and < 30 && magic_points >= 5)
        {
            state = State.Heal;
        }
        else if (rand < 20 && magic_points >= 5)
        {
            state = State.ElementalAttack;
        }
    }

    IEnumerator EndTurn()
    {
        state = State.Idle;
        
        yield return new WaitForSeconds(1f);
        
        manager.ChangeTurn();
    }

    private void Attack()
    {
        int rand = Random.Range(0, 100);

        multiplier = (rand <= 5)? 2: 1;

        player.TakeDamage(attack_power * multiplier);

        StartCoroutine(EndTurn());
    }

    private void Heal()
    {
        if (health_points < 100)
        {
            health_points += 5;
        } 
        else
        {
            health_points = 100;
        }
        
        hud.setEnemyHP(health_points);
        magic_points -= 5;
        
        StartCoroutine(EndTurn());
    }

    private void ElementalAttack()
    {
        int rand = Random.Range(0, 100);
        
        multiplier = (rand <= 5)? 2: 1;
        
        player.TakeDamage(attack_power * 2 * multiplier);
        magic_points -= 5;
        
        StartCoroutine(EndTurn());
    }

    public void TakeDamage(int damage)
    {
        health_points -= damage - defence_power;
        
        if (health_points <= 0)
        {
            health_points = 0;
            dead = true;
        } 
        
        hud.setEnemyHP(health_points);
        
        StartCoroutine(EndTurn());
    }
    
    public bool GetDead()
    {
        return dead;
    }
}
