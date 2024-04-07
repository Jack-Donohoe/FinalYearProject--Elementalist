using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Combat_Manager : MonoBehaviour
{
    public CombatHUD hud;
    
    public GameObject[] enemyTypes;

    public GameObject[] spawnedEnemies;

    private Vector3[] spawnLocations = new[]
    {
        new Vector3(-1.5f, 1.5f, 0f), 
        new Vector3(-0.5f, 1.5f, 0f), 
        new Vector3(0.5f, 1.5f, 0f), 
        new Vector3(1.5f, 1.5f, 0f),
    };

    enum BattleState { PlayerTurn, EnemyTurn, Player_Won, Player_Lost }

    private BattleState state;
    
    private Player_Combat player;

    private bool started = false;

    private void Start()
    {
        state = BattleState.PlayerTurn;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Combat>();
        
        player.StartTurn();
        hud.DialogueText.text = "Player Turn";
    }

    private void Update()
    {
        if (started)
        {
            if (state != BattleState.Player_Won && state != BattleState.Player_Lost)
            {
                if (player.Dead)
                {
                    state = BattleState.Player_Lost;
                    EndCombat();
                }

                if (spawnedEnemies[0].GetComponent<Grunt_Combat>().Dead)
                {
                    state = BattleState.Player_Won;
                    spawnedEnemies[0].SetActive(false);
                    EndCombat();
                }
            }
        }
    }
    
    public IEnumerator StartUp(Element enemyElement)
    {
        yield return new WaitForNextFrameUnit();
        Debug.Log("Starting Up...");
        SpawnEnemies(enemyElement, 1);
        started = true;
        hud.StartUp();
        player.SetEnemies();
    }

    void SpawnEnemies(Element enemyElement, int amount)
    {
        spawnedEnemies = new GameObject[amount];
        
        for (int i = 0; i < amount; i++)
        {
            Vector3 spawnLocation;
            
            if (amount == 1)
            {
                spawnLocation = new Vector3(0f, 1.5f, 0f);
            }
            else if (amount == 2)
            {
                spawnLocation = spawnLocations[i + 1];
            }
            else
            {
                spawnLocation = spawnLocations[i];
            }
            
            foreach (GameObject enemyType in enemyTypes)
            {
                if (enemyType.GetComponent<Grunt_Combat>().element == enemyElement)
                {
                    spawnedEnemies[i] = Instantiate(enemyType, spawnLocation, Quaternion.Euler(new Vector3(0f,180f,0f)));
                }
            }
        }
    }

    public void ChangeTurn()
    {
        if (state == BattleState.PlayerTurn)
        {
            state = BattleState.EnemyTurn;
            hud.DialogueText.text = "Enemy Turn";
            
            StartCoroutine(spawnedEnemies[0].GetComponent<Grunt_Combat>().StartTurn());
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
            hud.DialogueText.text = "Player Wins!";
            
            (int, int) combatRewards = player.CalculateCombatRewards();
            GameManager.Instance.SetPlayerResources(combatRewards.Item1, combatRewards.Item2);  
            
            StartCoroutine(GameManager.Instance.ReturnToLevel());
        } else if (state == BattleState.Player_Lost)
        {
            hud.DialogueText.text = "Player was Defeated";
            StartCoroutine(GameManager.Instance.LoseGame());
        }
    }

    public GameObject GetEnemy(int i)
    {
        return spawnedEnemies[i];
    }
}
