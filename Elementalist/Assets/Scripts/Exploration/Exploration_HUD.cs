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
    public Button[] Buttons;

    public void SetScoreText(string text)
    {
        scoreText.text = text;
    }

    public void InventoryButton()
    {
        LoadInventoryMenu();
    }

    private void LoadInventoryMenu()
    {
        Time.timeScale = 0f;
        inGameHUD.SetActive(false);
        
        List<Element> elements = GameManager.instance.GetElements();
        
        for(int i = 0; i < elements.Count; i++)
        {
            Buttons[i].GetComponentInChildren<TMP_Text>().text = elements[i].GetName();
        }
        
        inventoryMenu.SetActive(true);
    }
}
