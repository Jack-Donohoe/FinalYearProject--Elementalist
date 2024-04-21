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
    public Slider MPSlider;

    public GameObject CombatPanel, RewardPanel;

    public GameObject PauseMenu, PlayerInfo, ActionPanel, TargetPanel, ElementPanel;

    public GameObject RewardsScreen, LevelUpScreen;
    public TMP_Text EnemiesDefeated, XPGained, ElementAdded, LevelUpHP, LevelUpMP, LevelUpAttack, LevelUpDefence;

    public Button[] targetButtons, elementButtons;

    public Button backButton;

    public void StartUp()
    {
        PlayerHPSlider.maxValue = GameManager.Instance.Max_Health;
        MPSlider.maxValue = GameManager.Instance.Max_Magic;

        PlayerHPSlider.value = GameManager.Instance.HP;
        MPSlider.value = GameManager.Instance.MP;
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
        CombatPanel.SetActive(false);
        
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
        CombatPanel.SetActive(true);
    }
    
    public void TogglePlayerActions()
    {
        PlayerInfo.SetActive(!PlayerInfo.activeSelf);
        TargetPanel.SetActive(false);
        ActionPanel.SetActive(true);
    }
    
    public void RefreshTargetButtons(GameObject[] enemies)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            TMP_Text buttonText = targetButtons[i].transform.Find("Text").GetComponent<TMP_Text>();

            buttonText.text = !enemies[i].GetComponent<Grunt_Combat>().Dead ? enemies[i].GetComponent<Grunt_Combat>().enemy_Name : "";
        }
    }
    
    public void RefreshElementButtons()
    {
        List<Element> elements = GameManager.Instance.elementInventory;
        
        for (int i = 0; i < elements.Count; i++)
        {
            TMP_Text buttonText = elementButtons[i].transform.Find("Text").GetComponent<TMP_Text>();
            buttonText.text = elements[i].GetName();
        }
    }

    public IEnumerator ShowRewardsPanel(int enemiesDefeated, int xp, Element elementAdded)
    {
        yield return new WaitForSeconds(1.5f);
        CombatPanel.SetActive(false);
        RewardPanel.SetActive(true);
        
        EnemiesDefeated.text = "Enemies Defeated: " + enemiesDefeated;
        XPGained.text = "XP Gained: " + enemiesDefeated + " x " + xp /enemiesDefeated + " = " + xp + "xp";

        if (elementAdded != null)
        {
            ElementAdded.text = "Element Added: " + elementAdded.GetName();
        }
        else
        {
            ElementAdded.text = "Element Added: None";
        }
    }

    public IEnumerator ShowLevelUpPanel()
    {
        yield return new WaitForSeconds(1.5f);
        RewardsScreen.SetActive(false);
        LevelUpScreen.SetActive(true);
    }

    public void OnBackButton()
    {
        if (TargetPanel.activeSelf)
        {
            TargetPanel.SetActive(false);
        }
        else if (ElementPanel)
        {
            ElementPanel.SetActive(false);
        }
        
        ActionPanel.SetActive(true);
        backButton.gameObject.SetActive(false);
    }

    public void HideLevelUpPanel()
    {
        LevelUpScreen.SetActive(false);
        RewardsScreen.SetActive(true);
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

    public void OnReturnToLevelButton()
    {
        GameManager.Instance.ReturnToLevel();
    }

    public void setPlayerHP(int hp)
    {
       PlayerHPSlider.value = hp;
    }

    public void setMP(int mp)
    {
        MPSlider.value = mp;
    }
}
