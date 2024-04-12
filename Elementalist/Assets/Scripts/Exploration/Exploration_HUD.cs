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
    
    public Button[] elementButtons;
    public TMP_Text[] elementButtonTexts;
    public RawImage[] elementButtonImages;

    public TMP_Text playerLevel;
    public TMP_Text playerXP;
    
    public Slider PlayerHPSlider;
    public TMP_Text HPValue;
    public Slider MPSlider;
    public TMP_Text MPValue;

    public TMP_Text maxHP;
    public TMP_Text maxMP;
    public TMP_Text playerAttack;
    public TMP_Text playerDefence;

    public Button[] comboButtons;
    public RawImage[] comboButtonImages;
    public TMP_Text resultText;

    // Used to track first and second selected elements in combination menu
    private Element firstSelectedElement;
    private Element secondSelectedElement;
    private int selectedCounter;
    
    // Used to track first and second selected button in combination menu
    Button firstButton, secondButton;
    
    private void Start()
    {
        selectedCounter = 0;
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
        
        List<Element> elements = GameManager.Instance.elementInventory;
        
        for(int i = 0; i < elements.Count; i++)
        {
            elementButtons[i].gameObject.SetActive(true);
            elementButtonTexts[i].text = elements[i].GetName();
            elementButtonImages[i].texture = elements[i].GetIcon();

            string[] baseElements = {"Fire", "Water", "Earth", "Air"};

            comboButtons[i].GetComponent<Image>().color = new Color(0.35f, 0.35f, 0.35f);
            if (baseElements.Contains(elements[i].GetName()))
            {
                comboButtons[i].gameObject.SetActive(true);
                comboButtonImages[i].texture = elements[i].GetIcon();
            }
        }
        
        inventoryMenu.SetActive(true);
    }

    private void UpdateInventoryMenu()
    {
        List<Element> elements = GameManager.Instance.elementInventory;

        firstSelectedElement = null;
        secondSelectedElement = null;
        
        foreach (var button in elementButtons)
        {
            button.gameObject.SetActive(false);
        }
        
        foreach (var button in comboButtons)
        {
            button.gameObject.SetActive(false);
        }
        
        for(int i = 0; i < elements.Count; i++)
        {
            Debug.Log(elements[i].GetName());
            elementButtons[i].gameObject.SetActive(true);
            elementButtonTexts[i].text = elements[i].GetName();
            elementButtonImages[i].texture = elements[i].GetIcon();

            string[] baseElements = {"Fire", "Water", "Earth", "Air"};

            comboButtons[i].GetComponent<Image>().color = new Color(0.35f, 0.35f, 0.35f);
            if (baseElements.Contains(elements[i].GetName()))
            {
                comboButtons[i].gameObject.SetActive(true);
                comboButtonImages[i].texture = elements[i].GetIcon();
            }
        }
    }

    private void UnloadMenu(GameObject menu)
    {
        Time.timeScale = 1f;
        menu.SetActive(false);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inGameHUD.SetActive(true);
    }

    public void UpdateStatsMenu()
    {
        playerLevel.text = "Level: " + GameManager.Instance.playerLevel;
        playerXP.text = "Current XP: " + GameManager.Instance.playerXP + "/" + GameManager.Instance.xpToLevelUp;
        
        setPlayerHP(GameManager.Instance.HP, GameManager.Instance.Max_Health);
        setMP(GameManager.Instance.MP, GameManager.Instance.Max_Magic);

        maxHP.text = "Max HP: " + GameManager.Instance.Max_Health;
        maxMP.text = "Max MP: " + GameManager.Instance.Max_Magic;
        playerAttack.text = "Attack Power: " + GameManager.Instance.Attack_Power;
        playerDefence.text = "Defence Power: " + GameManager.Instance.Defence_Power;
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
        UpdateInventoryMenu();
    }
    
    public void OnToCombinationButton()
    {
        inventoryViews[0].SetActive(false);
        inventoryViews[1].SetActive(false);
        inventoryViews[2].SetActive(true);
        UpdateInventoryMenu();
    }
    
    public void OnToStatsButton()
    {
        inventoryViews[0].SetActive(true);
        inventoryViews[1].SetActive(false);
        inventoryViews[2].SetActive(false);
    }

    public void OnElementButton(Button button)
    {
        for (int i = 0; i < elementButtons.Length; i++)
        {
            if (elementButtons[i] == button)
            {
                GameManager.Instance.selectedElement = GameManager.Instance.GetElement(i);
                Debug.Log(GameManager.Instance.selectedElement.GetName());
            }
        }
    }

    public void OnCombineElementsButton(Button button)
    {
        for (int i = 0; i < comboButtons.Length; i++)
        {
            if (comboButtons[i] == button)
            {
                if (selectedCounter == 0)
                {
                    if (firstButton != null)
                    {
                        comboButtons[i].GetComponent<Image>().color = new Color(0.35f, 0.35f, 0.35f);
                    }
                    
                    firstButton = button;
                    firstButton.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
                    
                    selectedCounter++;
                    firstSelectedElement = GameManager.Instance.GetElement(i);
                    Debug.Log(firstSelectedElement);
                } else
                {
                    if (secondButton != null)
                    {
                        comboButtons[i].GetComponent<Image>().color = new Color(0.35f, 0.35f, 0.35f);
                    }
                    
                    secondButton = button;
                    secondButton.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
                    
                    selectedCounter = 0;
                    secondSelectedElement = GameManager.Instance.GetElement(i);
                    if (secondSelectedElement == firstSelectedElement)
                    {
                        firstSelectedElement = null;
                    }
                    Debug.Log(secondSelectedElement);
                }
            }
        }
    }

    public void OnCombineButton()
    {
        if (firstSelectedElement != null && secondSelectedElement != null)
        {
            Element newElement = ElementManager.Instance.CombineElements((firstSelectedElement.GetName(), secondSelectedElement.GetName()));

            if (!GameManager.Instance.elementInventory.Contains(newElement))
            {
                resultText.text = "You Created: " + newElement.GetName();

                Debug.Log(GameManager.Instance.AddElement(newElement));
                Debug.Log(GameManager.Instance.RemoveElement(firstSelectedElement));
                Debug.Log(GameManager.Instance.RemoveElement(secondSelectedElement));

                foreach (Element element in GameManager.Instance.elementInventory)
                {
                    if (element == newElement)
                    {
                        GameManager.Instance.selectedElement = element;
                    }
                }

                UpdateInventoryMenu();
            }
            else
            {
                resultText.text = newElement.GetName() + " already exists.";
            }
        }
    }
}
