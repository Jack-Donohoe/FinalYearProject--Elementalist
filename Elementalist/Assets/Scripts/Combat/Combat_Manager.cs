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

    private (int, string, int)[] enemyActions;

    private bool allEnemiesDead;

    private bool started = false;

    private void Start()
    {
        state = BattleState.PlayerTurn;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Combat>();
        
        player.StartTurn();
        hud.DialogueText.text = "Player Turn";

        allEnemiesDead = true;
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
                    StartCoroutine(EndCombat());
                }

                for (int i = 0; i < spawnedEnemies.Length; i++)
                {
                    if (spawnedEnemies[i].GetComponent<Grunt_Combat>().Dead)
                    {
                        allEnemiesDead = true;
                    }
                    else
                    {
                        allEnemiesDead = false;
                        return;
                    }
                }

                if (allEnemiesDead)
                {
                    state = BattleState.Player_Won;
                    StartCoroutine(EndCombat());
                }
            }
        }
    }

    public IEnumerator StartUp(Element enemyElement, bool eliteEnemy)
    {
        yield return new WaitForNextFrameUnit();
        
        int enemyAmount = Mathf.CeilToInt((float) GameManager.Instance.playerLevel / 2);

        if (eliteEnemy)
        {
            enemyAmount = 1;
        }
        
        SpawnEnemies(enemyElement, enemyAmount, eliteEnemy);
        
        started = true;
        hud.StartUp();
        player.SetEnemies();
        enemyActions = new (int, string, int)[spawnedEnemies.Length];
    }

    void SpawnEnemies(Element enemyElement, int amount, bool eliteEnemy)
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
                if (enemyType.GetComponent<Grunt_Combat>().element == enemyElement && enemyType.GetComponent<Grunt_Combat>().eliteEnemy == eliteEnemy)
                {
                    spawnedEnemies[i] = Instantiate(enemyType, spawnLocation, Quaternion.Euler(new Vector3(0f,180f,0f)));

                    Grunt_Combat gruntCombat = spawnedEnemies[i].GetComponent<Grunt_Combat>();

                    String enemyName = gruntCombat.element.GetName() + " " + gruntCombat.enemy_Name;

                    if (eliteEnemy == false)
                    {
                        enemyName = enemyName + " " + (i + 1);
                        Debug.Log(enemyName);
                    }
                    
                    spawnedEnemies[i].GetComponent<Grunt_Combat>().enemy_Name = enemyName;
                }
            }
        }
        hud.RefreshTargetButtons(spawnedEnemies);
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
            hud.RefreshTargetButtons(spawnedEnemies);
        }
    }

    IEnumerator EnemyTurn()
    {
        int aliveEnemyCount = 0;

        for (int i = 0; i < spawnedEnemies.Length; i++)
        {
            if (!spawnedEnemies[i].GetComponent<Grunt_Combat>().Dead)
            {
                aliveEnemyCount++;
            }
        }
        
        enemyActions = new (int, string, int)[aliveEnemyCount];

        int actionNum = 0;
        for (int i = 0; i < spawnedEnemies.Length; i++)
        {
            if (!spawnedEnemies[i].GetComponent<Grunt_Combat>().Dead)
            {
                (int, string) action = spawnedEnemies[i].GetComponent<Grunt_Combat>().SelectAction();
                enemyActions[actionNum] = (action.Item1, action.Item2, i);
                actionNum++;
            }
        }
        
        for (int i = 0; i < enemyActions.Length; i++)
        {
            (int, string, int) action = enemyActions[i];
            hud.DialogueText.text = spawnedEnemies[action.Item3].GetComponent<Grunt_Combat>().enemy_Name + "'s Turn";
            
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

    IEnumerator EndCombat()
    {
        yield return new WaitForSeconds(1f);
        if (state == BattleState.Player_Won)
        {
            hud.DialogueText.text = "Player Wins!";
            
            (int, int, int) combatRewards = player.CalculateCombatRewards();
            GameManager.Instance.SetPlayerResources(combatRewards.Item1, combatRewards.Item2);
            int xpToAdd = combatRewards.Item3;

            Element elementToAdd;

            if (!GameManager.Instance.elementInventory.Contains(spawnedEnemies[0].GetComponent<Grunt_Combat>().element))
            {
                elementToAdd = spawnedEnemies[0].GetComponent<Grunt_Combat>().element;
                GameManager.Instance.elementInventory.Add(elementToAdd);
            }
            else
            {
                elementToAdd = null;
            }
            
            StartCoroutine(hud.ShowRewardsPanel(spawnedEnemies.Length, xpToAdd, elementToAdd));

            if (GameManager.Instance.CheckLevelUp(xpToAdd))
            {
                StartCoroutine(hud.ShowLevelUpPanel());
            }
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
