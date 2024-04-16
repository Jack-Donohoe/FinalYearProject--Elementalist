using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueMenu : MonoBehaviour
{
    private Exploration_HUD HUD;
    
    public TMP_Text dialogueText;

    private string tutorial = 
        "Hello! Welcome to Elementalist." +
        "\n\nThe goal in this game is to defeat enemies, grow in strength and find and combine new elements in order to defeat each of the three floor guardians blocking the way. " +
        "\n\nIf you do this, you'll complete the game! So go forth brave adventurer, and good luck!";

    private string inventorytutorial =
        "This is your inventory menu. It contains three separate tabs, which you can navigate between with the arrow buttons located at the top of each tab. " +
        "\n\nThe first is the player info tab, which shows information about the player's level and XP (how close they are to levelling up), current health and magic (HP and MP respectively) and the player's current statistics or stats." +
        "\n\nThe second is the element inventory, which shows the up to four elements you currently possess. Elements can be removed down to a minimum of one, and you need to have an empty slot in order to gain a new element from a defeated enemy." +
        "\n\nThe final tab is the element combination menu. This menu allows you to combine two of the base elements of Fire,Water,Earth and Air into a new combination element which will be more powerful, but cannot be combined again. " +
        "Combining two elements will use them up and you will need to reacquire them in order to use them again, so be mindful.";
    
    // Start is called before the first frame update
    void Start()
    {
        dialogueText.text = tutorial;
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
