using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hover_On_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CombatHUD HUD;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.CompareTag("Attack Button"))
        {
            HUD.DialogueText.text = "Basic Attack" + "\nMP Cost: None";
        }
        else if (gameObject.CompareTag("Elemental Button"))
        {
            Element selectedElement = GameManager.Instance.selectedElement;

            HUD.DialogueText.text = selectedElement.GetAttackName() + "\nMP Cost: " + selectedElement.GetMagicCost();
        }
        else if (gameObject.CompareTag("Change Element Button"))
        {
            HUD.DialogueText.text = "Change Element" + "\nOnce Per Turn";
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HUD.DialogueText.text = "Player Turn";
    }
}
