using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Combat_Manager : MonoBehaviour
{
    public CombatHUD hud;
    
    public GameObject[] enemyTypes;

    public GameObject[] spawnedEnemies;

    private Vector3[] spawnLocations =
    {
        new (-4.5f, 1.5f, 0f), 
        new (-1.5f, 1.5f, 0f), 
        new (1.5f, 1.5f, 0f), 
        new (4.5f, 1.5f, 0f),
    };

    enum BattleState { PlayerTurn, EnemyTurn, Player_Won, Player_Lost }

    private BattleState state;
    
    private Player_Combat player;

    private (int, String)[] enemyActions;

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
        SpawnEnemies(enemyElement, 2);
        
        started = true;
        hud.StartUp();
        player.SetEnemies();
        enemyActions = new (int, string)[spawnedEnemies.Length];
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

                    String enemy_Name = spawnedEnemies[i].GetComponent<Grunt_Combat>().enemy_Name + " " +  (i + 1);
                    spawnedEnemies[i].GetComponent<Grunt_Combat>().enemy_Name = enemy_Name;
                    hud.targetButtonTexts[i].GetComponent<TMP_Text>().text = enemy_Name;
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

            StartCoroutine(EnemyTurn());
        } else if (state == BattleState.EnemyTurn)
        {
            state = BattleState.PlayerTurn;
            hud.DialogueText.text = "Player Turn";
            
            player.StartTurn();
        }
    }

    IEnumerator EnemyTurn()
    {
        for (int i = 0; i < enemyActions.Length; i++)
        {
            enemyActions[i] = spawnedEnemies[i].GetComponent<Grunt_Combat>().SelectAction();
        }
        
        for (int i = 0; i < enemyActions.Length; i++)
        {
            hud.DialogueText.text = spawnedEnemies[i].GetComponent<Grunt_Combat>().enemy_Name + "'s Turn";
            
            (int, string) action = enemyActions[i];
            StartCoroutine(EnemyAction(action.Item1, action.Item2, i));
            yield return new WaitForSecondsRealtime(1.5f);
        }
    }

    IEnumerator EnemyAction(int damage, string dialogue, int counter)
    {
        yield return new WaitForSecondsRealtime(1f);
        player.TakeDamage(damage);
        hud.DialogueText.text = dialogue;

        counter++;

        if (counter == enemyActions.Length)
        {
            yield return new WaitForSecondsRealtime(1f);
            ChangeTurn();
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
