using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Exploration_HUD : MonoBehaviour
{
    public GameObject loadingScreen;
    public GameObject inGameHUD;
    public TMP_Text scoreText;
    public GameObject pauseMenu;
    public GameObject inventoryMenu;
    public Button[] buttons;
    public TMP_Text[] buttonTexts;
    public TMP_Text selectedElementText;
    public Slider PlayerHPSlider;
    public Slider MPSlider;

    private void Start()
    {
        selectedElementText.text = "Selected Element: " + GameManager.Instance.selectedElement.GetName();
    }

    public void SetScoreText(string text)
    {
        scoreText.text = text;
    }

    public void HandlePauseMenu()
    {
        if (pauseMenu.activeSelf == false)
        {
            LoadPauseMenu();
        }
        else
        {
            UnloadMenu(pauseMenu);
        }
    }
    
    private void LoadPauseMenu()
    {
        Time.timeScale = 0f;
        inGameHUD.SetActive(false);
        
        if (inventoryMenu.activeSelf)
        {
            inventoryMenu.SetActive(false);
        }
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        pauseMenu.SetActive(true);
    }

    public void HandleInventoryMenu()
    {
        if (inventoryMenu.activeSelf == false)
        {
            LoadInventoryMenu();
        }
        else
        {
            UnloadMenu(inventoryMenu);
        }
    }

    private void LoadInventoryMenu()
    {
        Time.timeScale = 0f;
        inGameHUD.SetActive(false);

        if (pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
        }
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        List<Element> elements = GameManager.Instance.GetElements();
        
        for(int i = 0; i < elements.Count; i++)
        {
            buttons[i].gameObject.SetActive(true);
            buttonTexts[i].text = elements[i].GetName();
        }
        
        inventoryMenu.SetActive(true);
    }

    private void UnloadMenu(GameObject menu)
    {
        Time.timeScale = 1f;
        menu.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inGameHUD.SetActive(true);
    }
    
    public void setPlayerHP(int hp)
    {
        PlayerHPSlider.value = hp;
    }
    
    public void setMP(int mp)
    {
        MPSlider.value = mp;
    }

    public void OnSaveGameButton()
    {
        (int, int, int, int) playerInfo = GameManager.Instance.GetPlayerInfo();
        GameObject map = GameManager.Instance.GetMap();
        
        GameData gameData = new GameData
        {
            playerPos = GameObject.FindGameObjectWithTag("Player").transform.position,
            playerRotation = GameObject.FindGameObjectWithTag("Player").transform.rotation,
            playerHealth = playerInfo.Item1,
            playerMagic = playerInfo.Item2,
            playerAttack = playerInfo.Item3,
            playerDefence = playerInfo.Item4,
            levelSize = map.GetComponent<ProcGenV3>().GetIds().Length,
            ids = map.GetComponent<ProcGenV3>().GetIds(),
            roomsCompleted = map.GetComponent<ProcGenV3>().GetRoomsCompleted(),
            roomTypes = map.GetComponent<ProcGenV3>().GetRoomTypes()
        };
        DataManager.instance.SaveGameData(gameData);
    }

    public void OnLoadGameButton()
    {
        
    }

    public void OnMainMenuButton()
    {
        UnloadMenu(pauseMenu);
        GameManager.Instance.LoadMainMenu();
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnElementButton(Button button)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == button)
            {
                GameManager.Instance.selectedElement = GameManager.Instance.GetElement(i);
                selectedElementText.text = "Selected Element: " + GameManager.Instance.selectedElement.GetName();
            }
        }
    }
}
