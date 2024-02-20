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

    private int damage;
    
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
    
    private IEnumerator EndTurn()
    {
        state = State.Idle;
        yield return new WaitForSeconds(2f);
        manager.ChangeTurn();
    }
    
    private void Attack()
    {
        int rand = Random.Range(0, 100);

        multiplier = (rand <= 5)? 2: 1;

        damage = attack_power * multiplier;
        enemies[0].GetComponent<Grunt_Combat>().TakeDamage(damage);
        hud.DialogueText.text = "Player attacks and deals " + damage + " damage to Enemy A";
        
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
        
        Debug.Log(health_points);
        hud.setPlayerHP(health_points);
        
        magic_points -= 5;
        hud.setMP(magic_points);
        hud.DialogueText.text = "Player heals and restores 5HP";
        
        StartCoroutine(EndTurn());
    }

    private void ElementalAttack()
    {
        int rand = Random.Range(0, 100);
        
        multiplier = (rand <= 5)? 2: 1;

        Element element = GameManager.Instance.selectedElement;
        GameObject projectile = Instantiate(element.GetProjectile(),transform.position, Quaternion.identity);

        Vector3 direction = (enemies[0].transform.position - projectile.transform.position).normalized * (element.GetProjectileSpeed() * Time.deltaTime);
        projectile.transform.LookAt(enemies[0].transform.position);
        projectile.GetComponent<Projectile>().SetMoveDirection(direction);
        
        magic_points -= 5;
        hud.setMP(magic_points);
        
        damage = attack_power * element.GetDamageValue() * multiplier;
        hud.DialogueText.text = "Player uses " + element.GetAttackName() + " and deals " + damage + " damage to Enemy A";
        
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
        
        hud.setPlayerHP(health_points);
    }

    public bool IsDead()
    {
        return dead;
    }

    public int GetDamage()
    {
        return damage;
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
