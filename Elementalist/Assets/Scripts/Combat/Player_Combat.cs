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
    
    public enum State { Idle, Ready, Attack, Heal, ElementalAttack, Dead }

    public State state;
    
    private bool dead = false;

    private GameObject[] enemies;

    private int damage;
    
    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;

        (int, int, int, int) playerInfo = GameManager.Instance.GetPlayerInfo();
        health_points = playerInfo.Item1;
        magic_points = playerInfo.Item2;
        attack_power = playerInfo.Item3;
        defence_power = playerInfo.Item4;
        
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

    public void StartTurn()
    {
        state = State.Ready;
    }
    
    public void EndTurn()
    {
        //yield return new WaitForSeconds(1f);
        Debug.Log("Ending Player Turn");
        GameManager.Instance.SetPlayerInfo((health_points,magic_points,attack_power,defence_power));
        manager.ChangeTurn();
    }
    
    private void Attack()
    {
        int rand = Random.Range(0, 100);

        multiplier = (rand <= 8)? 2: 1;

        damage = attack_power * multiplier - enemies[0].GetComponent<Grunt_Combat>().GetDefencePower();
        enemies[0].GetComponent<Grunt_Combat>().TakeDamage(damage);
        string textToDisplay = "Player attacks and deals " + damage + " damage!";
        
        if (multiplier == 2)
        {
            textToDisplay += " Critical Hit!";
        }

        hud.DialogueText.text = textToDisplay;

        state = State.Idle;
        StartCoroutine(StartEndTurn());
    }

    private IEnumerator StartEndTurn()
    {
        yield return new WaitForSeconds(1.5f);
        EndTurn();
    }

    private void Heal()
    {
        int healthToRestore = (Random.value > 0.7f) ? 10 : 5;
        health_points += healthToRestore;
        
        if (health_points > 100)
        {
            health_points = 100;
        }
        
        hud.setPlayerHP(health_points);
        
        magic_points -= 5;
        hud.setMP(magic_points);
        hud.DialogueText.text = "Player uses Heal and restores " + healthToRestore + "HP.";
        
        state = State.Idle;
        StartCoroutine(StartEndTurn());
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
        
        magic_points -= 10;
        hud.setMP(magic_points);
        
        damage = attack_power * element.GetDamageValue() * multiplier - enemies[0].GetComponent<Grunt_Combat>().GetDefencePower();
        string textToDisplay = "Player attacks with a " + element.GetAttackName() + " and deals " + damage + " damage!";

        if (multiplier == 2)
        {
            textToDisplay += " Critical Hit!";
        }

        hud.DialogueText.text = textToDisplay;
        
        state = State.Idle;
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

    public bool IsDead()
    {
        return dead;
    }

    public int GetDamage()
    {
        return damage;
    }

    public int GetDefencePower()
    {
        return defence_power;
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
