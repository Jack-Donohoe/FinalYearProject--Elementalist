using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat_Manager : MonoBehaviour
{
    public CombatHUD hud;
    
    public GameObject[] enemies;

    enum BattleState { PlayerTurn, EnemyTurn, Player_Won, Player_Lost }

    private BattleState state;
    
    private Player_Combat player;

    private void Awake()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.PlayerTurn;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Combat>();
        player.StartTurn();
        hud.DialogueText.text = "Player Turn";
    }

    private void Update()
    {
        if (player.IsDead())
        {
            state = BattleState.Player_Lost;
            EndCombat();
        }

        if (enemies[0].GetComponent<Grunt_Combat>().IsDead())
        {
            state = BattleState.Player_Won;
            EndCombat();
        }
    }

    public void ChangeTurn()
    {
        if (state == BattleState.PlayerTurn)
        {
            state = BattleState.EnemyTurn;
            hud.DialogueText.text = "Enemy Turn";
            
            StartCoroutine(enemies[0].GetComponent<Grunt_Combat>().StartTurn());
        } else if (state == BattleState.EnemyTurn)
        {
            state = BattleState.PlayerTurn;
            hud.DialogueText.text = "Player Turn";
            
            player.StartTurn();
        }
    }

    void EndCombat()
    {
        if (state == BattleState.Player_Won)
        {
            hud.DialogueText.text = "Player Won";
            GameManager.instance.ReturnToLevel();
        } else if (state == BattleState.Player_Lost)
        {
            hud.DialogueText.text = "Player Lost";
            GameManager.instance.LoseGame();
        }
    }
}
