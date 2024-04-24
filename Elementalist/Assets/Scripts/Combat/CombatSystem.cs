using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class CombatSystem : MonoBehaviour
{
    public CombatHUD hud;

    int playerHP = 100;
    int playerMP = 100;
    int enemyHP = 100;

    public TMP_Text dialogueText;

    public BattleState state;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        dialogueText.text = "Start";
        StartCoroutine(playerTurn());
    }

    IEnumerator playerTurn()
    {
        yield return new WaitForSeconds(2f);
        state = BattleState.PLAYERTURN;
        dialogueText.text = "Player Turn";
    }

    IEnumerator playerAttack()
    {
        enemyHP = enemyHP - 5;

        yield return new WaitForSeconds(1f);

        if (enemyHP <= 0)
        {
            state = BattleState.WON;
            endBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(enemyTurn());
        }
    }

    IEnumerator fireballAttack()
    {
        enemyHP = enemyHP - 10;

        playerMP = playerMP - 10;
        hud.setMP(playerMP);

        yield return new WaitForSeconds(1f);

        if (enemyHP <= 0)
        {
            state = BattleState.WON;
            endBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(enemyTurn());
        }
    }

    IEnumerator playerHeal()
    {
        if (playerHP < 100)
        {
            playerHP = playerHP + 5;
        } 
        else
        {
            playerHP = 100;
        }

        hud.setPlayerHP(playerHP);

        yield return new WaitForSeconds(1f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(enemyTurn());
    }

    IEnumerator enemyTurn()
    {
        dialogueText.text = "Enemy Turn";

        playerHP = playerHP - 5;
        hud.setPlayerHP(playerHP);

        yield return new WaitForSeconds(0.5f);
        if (playerHP <= 0)
        {
            state = BattleState.LOST;
            endBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            StartCoroutine(playerTurn());
        }
    }

    void endBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "Player Won";
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "Player Lost";
        }
    }

    public void onAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(playerAttack());
    }

    public void onFireballButton()
    {
        if (state != BattleState.PLAYERTURN || playerMP == 0)
            return;

        StartCoroutine(fireballAttack());
    }

    public void onHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        StartCoroutine(playerHeal());
    }
}
