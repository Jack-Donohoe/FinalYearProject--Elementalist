using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueMenu : MonoBehaviour
{
    private Exploration_HUD HUD;
    
    public TMP_Text dialogueText;

    public GameObject inventoryTutorialMenu;
    public TMP_Text inventoryText;

    private string tutorial = 
        "Hello! Welcome to Elementalist." +
        "\n\nThe goal in this game is to defeat enemies, grow in strength and find and combine new elements in order to defeat each of the three floor guardians blocking the way. " +
        "\n\nIf you do this, you'll complete the game! So go forth and good luck!";

    private string inventorytutorial =
        "This is your inventory menu. It contains three separate tabs, which you can navigate between with the arrow buttons located at the top of each tab. " +
        "\n\nThe first is the player info tab, which shows information about the player's level and XP, current health and magic and the player's current stats." +
        "\n\nThe second is the element inventory, which shows the up to four elements you currently possess. Elements can be removed down to a minimum of one, and you need to have an empty slot in order to gain a new element from a defeated enemy." +
        "\n\nThe final tab is the element combination menu. This menu allows you to combine two of the base elements of Fire,Water,Earth and Air into a new combination element. " +
        "Combining two elements will use them up and you will need to reacquire them in order to use them again.";
    
    // Start is called before the first frame update
    void Start()
    {
        dialogueText.text = tutorial;
        inventoryText.text = inventorytutorial;
    }

    public void OnCloseDialogueButton()
    {
        HUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<Exploration_HUD>();
        
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        HUD.dialogueMenu.SetActive(false);
        HUD.inGameHUD.SetActive(true);
    }

    public void OnCloseInventoryTutorialButton()
    {
        inventoryTutorialMenu.SetActive(false);
    }
}
