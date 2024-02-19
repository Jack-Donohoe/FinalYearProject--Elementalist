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

    public void OnMainMenuButton()
    {
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
