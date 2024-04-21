using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Exploration_HUD : MonoBehaviour
{
    public GameObject loadingScreen;
    public GameObject inGameHUD;
    
    public GameObject pauseMenu;
    
    public GameObject inventoryMenu;
    public GameObject[] inventoryViews;

    public GameObject dialogueMenu;

    public TMP_Text interactText;
    
    public IEnumerator RemoveLoadingScreen(bool tutorial)
    {
        yield return new WaitForSeconds(1.5f);
        loadingScreen.gameObject.SetActive(false);

        if (tutorial)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            dialogueMenu.SetActive(true);
        }
        else
        {
            inGameHUD.SetActive(true);
        }
    }

    public void HandlePauseMenu()
    {
        if (pauseMenu.activeSelf == false)
        {
            LoadPauseMenu();
        }
        else
        {
            pauseMenu.GetComponent<PauseMenu>().SettingsPanel.SetActive(false);
            pauseMenu.GetComponent<PauseMenu>().OptionsPanel.SetActive(true);
            
            UnloadMenu(pauseMenu);
        }
    }
    
    private void LoadPauseMenu()
    {
        Time.timeScale = 0f;
        inGameHUD.SetActive(false);
        
        if (dialogueMenu.activeSelf)
        {
            dialogueMenu.SetActive(false);
        }
        
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
            ShowInventoryMenu();
        }
        else
        {
            UnloadMenu(inventoryMenu);
        }
    }

    private void ShowInventoryMenu()
    {
        Time.timeScale = 0f;
        inGameHUD.SetActive(false);
        
        if (dialogueMenu.activeSelf)
        {
            dialogueMenu.SetActive(false);
        }

        if (pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
        }

        if (GameManager.Instance.levelName == "Level1")
        {
            bool tutorial = GameManager.Instance.tutorial;
            dialogueMenu.GetComponent<DialogueMenu>().inventoryTutorialMenu.SetActive(tutorial);
        }

        InventoryMenu menu = inventoryMenu.GetComponent<InventoryMenu>();
        
        menu.UpdateStatsMenu();
        menu.LoadInventoryMenu();
        
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

    public void HidePauseMenu()
    {
        UnloadMenu(pauseMenu);
    }

    public void OnToElementsButton()
    {
        inventoryViews[0].SetActive(false);
        inventoryViews[1].SetActive(true);
        inventoryViews[2].SetActive(false);
        
        inventoryMenu.GetComponent<InventoryMenu>().UpdateInventoryMenu();
        dialogueMenu.GetComponent<DialogueMenu>().inventoryTutorialMenu.SetActive(false);
    }
    
    public void OnToCombinationButton()
    {
        inventoryViews[0].SetActive(false);
        inventoryViews[1].SetActive(false);
        inventoryViews[2].SetActive(true);
        
        inventoryMenu.GetComponent<InventoryMenu>().UpdateInventoryMenu();
        dialogueMenu.GetComponent<DialogueMenu>().inventoryTutorialMenu.SetActive(false);
    }
    
    public void OnToStatsButton()
    {
        inventoryViews[0].SetActive(true);
        inventoryViews[1].SetActive(false);
        inventoryViews[2].SetActive(false);
        
        dialogueMenu.GetComponent<DialogueMenu>().inventoryTutorialMenu.SetActive(false);
    }
}
