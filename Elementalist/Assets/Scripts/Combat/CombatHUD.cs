using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatHUD : MonoBehaviour
{
    public TMP_Text DialogueText; 
    public Slider PlayerHPSlider;
    public Slider EnemyHPSlider;
    public Slider MPSlider;

    public GameObject CombatUI, PauseMenu, AttackButton, ElementalAttackButton, HealButton;

    public void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerHPSlider.maxValue = player.GetComponent<Player_Combat>().Max_Health;
        MPSlider.maxValue = player.GetComponent<Player_Combat>().Max_Magic;

        GameObject enemy = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<Combat_Manager>().GetEnemy(0);
        EnemyHPSlider.maxValue = enemy.GetComponent<Grunt_Combat>().HP;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandlePauseMenu();
        }
    }
    
    public void HandlePauseMenu()
    {
        if (PauseMenu.activeSelf == false)
        {
            LoadPauseMenu();
        }
        else
        {
            UnloadMenu(PauseMenu);
        }
    }
    
    private void LoadPauseMenu()
    {
        Time.timeScale = 0f;
        CombatUI.SetActive(false);
        
        if (PauseMenu.activeSelf)
        {
            PauseMenu.SetActive(false);
        }
        
        PauseMenu.SetActive(true);
    }
    
    private void UnloadMenu(GameObject menu)
    {
        Time.timeScale = 1f;
        menu.SetActive(false);
        CombatUI.SetActive(true);
    }
    
    public void OnMainMenuButton()
    {
        UnloadMenu(PauseMenu);
        GameManager.Instance.LoadMainMenu();
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void setPlayerHP(int hp)
    {
       PlayerHPSlider.value = hp;
    }

    public void setEnemyHP(int hp)
    {
        EnemyHPSlider.value = hp;
    }

    public void setMP(int mp)
    {
        MPSlider.value = mp;
    }

    public void TogglePlayerActions()
    {
        AttackButton.SetActive(!AttackButton.activeSelf);
        ElementalAttackButton.SetActive(!ElementalAttackButton.activeSelf);
        HealButton.SetActive(!HealButton.activeSelf);
    }
}
