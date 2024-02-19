using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Exploration_HUD : MonoBehaviour
{
    public GameObject inGameHUD;
    public TMP_Text scoreText;
    public GameObject inventoryMenu;
    public Button[] buttons;
    public TMP_Text[] buttonTexts;

    public void SetScoreText(string text)
    {
        scoreText.text = text;
    }

    public void HandleInventoryMenu()
    {
        if (inventoryMenu.activeSelf == false)
        {
            LoadInventoryMenu();
        }
        else
        {
            UnloadInventoryMenu();
        }
    }

    private void LoadInventoryMenu()
    {
        Time.timeScale = 0f;
        inGameHUD.SetActive(false);
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        List<Element> elements = GameManager.instance.GetElements();
        
        for(int i = 0; i < elements.Count; i++)
        {
            buttonTexts[i].text = elements[i].GetName();
        }
        
        inventoryMenu.SetActive(true);
    }

    private void UnloadInventoryMenu()
    {
        Time.timeScale = 1f;
        inventoryMenu.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inGameHUD.SetActive(true);
    }

    public OnElementButton()
    {
        foreach (var button in buttons)
        {
            if (gameObject )
        }
    }
}
