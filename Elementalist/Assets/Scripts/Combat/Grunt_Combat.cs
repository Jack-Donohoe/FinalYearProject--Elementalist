using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt_Combat : MonoBehaviour
{
    private int health_points = 40;
    private int magic_points = 20;
    private int attack_power = 5;
    private int defence_power = 2;
    
    public CombatHUD hud;
    
    public enum State
    {
        Idle,
        Attack,
        Heal,
        ElementalAttack
    }

    private bool dead = false;

    public State state;
    
    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        int multiplier;

        int rand = Random.Range(0, 100);

        multiplier = (rand <= 5)? 2: 1;

        //player.TakeDamage(attack_power * multiplier);
    }

    public void Heal()
    {
        health_points = health_points + 5;
    }

    public void ElementalAttack()
    {
        
    }
}
