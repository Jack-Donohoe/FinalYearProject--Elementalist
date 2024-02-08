using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Combat : MonoBehaviour
{
    private int health_points = 100;
    private int magic_points = 50;
    private int attack_power = 10;
    private int defence_power = 5;
    
    private int multiplier;
    
    public CombatHUD hud;

    public Combat_Manager manager;
    
    public enum State { Idle, Ready, Attack, Heal, ElementalAttack }

    public State state;
    
    private bool dead = false;

    private GameObject[] enemies;
    
    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
        
        hud.setPlayerHP(health_points);
        hud.setMP(magic_points);

        enemies = manager.enemies;
        Debug.Log(enemies.Length);
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

    public void StartTurn()
    {
        state = State.Ready;
    }
    
    private void EndTurn()
    {
        state = State.Idle;
        
        manager.ChangeTurn();
    }
    
    private void Attack()
    {
        int rand = Random.Range(0, 100);

        multiplier = (rand <= 5)? 2: 1;

        enemies[0].GetComponent<Grunt_Combat>().TakeDamage(attack_power * multiplier);
        
        EndTurn();
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
        
        Debug.Log(health_points);
        hud.setPlayerHP(health_points);
        
        magic_points -= 5;
        hud.setMP(magic_points);
        
        EndTurn();
    }

    private void ElementalAttack()
    {
        int rand = Random.Range(0, 100);
        
        multiplier = (rand <= 5)? 2: 1;

        GameObject projectile = Instantiate(GameManager.instance.GetElement(0).GetProjectile(),transform.position, Quaternion.identity);

        Vector3 direction = (enemies[0].transform.position - projectile.transform.position).normalized * (5f * Time.deltaTime);
        projectile.transform.LookAt(enemies[0].transform.position);
        projectile.GetComponent<Projectile>().SetMoveDirection(direction);
        
        enemies[0].GetComponent<Grunt_Combat>().TakeDamage(attack_power * GameManager.instance.GetElement(0).GetDamageValue() * multiplier);
        
        magic_points -= 5;
        hud.setMP(magic_points);
        
        EndTurn();
    }

    public void TakeDamage(int damage)
    {
        health_points -= damage - defence_power;
        
        if (health_points <= 0)
        {
            health_points = 0;
            dead = true;
        } 
        
        hud.setPlayerHP(health_points);
    }

    public bool IsDead()
    {
        return dead;
    }
    
    public void onAttackButton()
    {
        if (state != State.Ready)
            return;

        Attack();
    }

    public void onFireballButton()
    {
        if (state != State.Ready || magic_points == 0)
            return;

        ElementalAttack();
    }

    public void onHealButton()
    {
        if (state != State.Ready || magic_points == 0)
            return;

        Heal();
    }
}
