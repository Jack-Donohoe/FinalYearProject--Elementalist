using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    private Exploration_HUD HUD;

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

    public Button[] elementButtons;
    public TMP_Text[] elementButtonTexts;
    public RawImage[] elementButtonImages;
    
    public Button[] comboButtons;
    public RawImage[] comboButtonImages;
    public TMP_Text resultText;

    // Used to track first and second selected elements in combination menu
    private Element firstSelectedElement;
    private Element secondSelectedElement;
    private int selectedCounter;
    
    // Used to track first and second selected button in combination menu
    Button firstButton, secondButton;

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
    
    public void LoadInventoryMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        List<Element> elements = GameManager.Instance.elementInventory;
        
        for(int i = 0; i < elements.Count; i++)
        {
            elementButtons[i].gameObject.SetActive(true);
            elementButtonTexts[i].text = elements[i].GetName();
            elementButtonImages[i].texture = elements[i].GetIcon();
            elementButtons[i].GetComponent<Image>().color = new Color(0.35f, 0.35f, 0.35f);
            
            if (GameManager.Instance.elementInventory[i] == GameManager.Instance.selectedElement)
            {
                elementButtons[i].GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
            }

            List<string> baseElements = new List<string>{"Fire", "Water", "Earth", "Air"};

            comboButtons[i].GetComponent<Image>().color = new Color(0.35f, 0.35f, 0.35f);
            if (baseElements.Contains(elements[i].GetName()))
            {
                comboButtons[i].gameObject.SetActive(true);
                comboButtonImages[i].texture = elements[i].GetIcon();
            }
        }
    }

    public void UpdateInventoryMenu()
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
            elementButtons[i].gameObject.SetActive(true);
            elementButtonTexts[i].text = elements[i].GetName();
            elementButtonImages[i].texture = elements[i].GetIcon();
            elementButtons[i].GetComponent<Image>().color = new Color(0.35f, 0.35f, 0.35f);
            
            if (GameManager.Instance.elementInventory[i] == GameManager.Instance.selectedElement)
            {
                elementButtons[i].GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
            }

            List<string> baseElements = new List<string>{"Fire", "Water", "Earth", "Air"};

            comboButtons[i].GetComponent<Image>().color = new Color(0.35f, 0.35f, 0.35f);
            if (baseElements.Contains(elements[i].GetName()))
            {
                comboButtons[i].gameObject.SetActive(true);
                comboButtonImages[i].texture = elements[i].GetIcon();
            }
        }
    }
    
    public void setPlayerHP(int hp, int maxHealth)
    {
        PlayerHPSlider.maxValue = maxHealth;
        PlayerHPSlider.value = hp;
        HPValue.text = hp + "/" + maxHealth;
    }
    
    public void setMP(int mp, int maxMagic)
    {
        MPSlider.maxValue = maxMagic;
        MPSlider.value = mp;
        MPValue.text = mp + "/" + maxMagic;
    }
    
    public void OnElementButton(Button button)
    {
        for (int i = 0; i < elementButtons.Length; i++)
        {
            elementButtons[i].GetComponent<Image>().color = new Color(0.35f, 0.35f, 0.35f);
            if (elementButtons[i] == button)
            {
                elementButtons[i].GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
                GameManager.Instance.selectedElement = GameManager.Instance.GetElement(i);
                Debug.Log(GameManager.Instance.selectedElement.GetName());
            }
        }
    }

    public void OnRemoveElementButton()
    {
        List<Element> elements = GameManager.Instance.elementInventory;
        if (elements.Count > 1)
        {
            GameManager.Instance.elementInventory.Remove(GameManager.Instance.selectedElement);
            GameManager.Instance.selectedElement = GameManager.Instance.GetElement(0);
            UpdateInventoryMenu();
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
                        firstButton.GetComponent<Image>().color = new Color(0.35f, 0.35f, 0.35f);
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
                        secondButton.GetComponent<Image>().color = new Color(0.35f, 0.35f, 0.35f);
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
