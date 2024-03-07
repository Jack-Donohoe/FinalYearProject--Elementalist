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

    public GameObject CombatUI, PauseMenu;

    public void Start()
    {
        PlayerHPSlider.maxValue = 100;
        EnemyHPSlider.maxValue = 40;
        MPSlider.maxValue = 50;
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
}
