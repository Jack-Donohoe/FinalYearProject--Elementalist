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
    public GameObject[] inventoryViews;
    public Button[] buttons;
    public TMP_Text[] buttonTexts;
    public RawImage[] buttonImages;
    public Slider PlayerHPSlider;
    public TMP_Text HPValue;
    public Slider MPSlider;
    public TMP_Text MPValue;
    
    
    private void Start()
    {
        
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
            buttonImages[i].texture = elements[i].GetIcon();
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
    
    public void setPlayerHP(int hp, int maxHP)
    {
        PlayerHPSlider.value = hp;
        HPValue.text = hp + "/" + maxHP;
    }
    
    public void setMP(int mp, int maxMP)
    {
        MPSlider.value = mp;
        MPValue.text = mp + "/" + maxMP;
    }

    public void OnSaveGameButton()
    {
        GameManager.Instance.SaveGame();
    }

    public void OnLoadGameButton()
    {
        UnloadMenu(pauseMenu);
        GameManager.Instance.StartLoadGame();
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

    public void OnToElementsButton()
    {
        inventoryViews[0].SetActive(false);
        inventoryViews[1].SetActive(true);
        inventoryViews[2].SetActive(false);
    }
    
    public void OnToCombinationButton()
    {
        inventoryViews[0].SetActive(false);
        inventoryViews[1].SetActive(false);
        inventoryViews[2].SetActive(true);
    }
    
    public void OnToStatsButton()
    {
        inventoryViews[0].SetActive(true);
        inventoryViews[1].SetActive(false);
        inventoryViews[2].SetActive(false);
    }

    public void OnElementButton(Button button)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == button)
            {
                GameManager.Instance.selectedElement = GameManager.Instance.GetElement(i);
                Debug.Log(GameManager.Instance.selectedElement.GetName());
            }
        }
    }

    public void OnCombineButton()
    {
        Element element1 = GameManager.Instance.GetElement(0);
        Element element2 = GameManager.Instance.GetElement(1);
        
        Element newElement = ElementManager.Instance.CombineElements((element1.GetName(),element2.GetName()));
        Debug.Log(GameManager.Instance.AddElement(newElement));
    }
}
