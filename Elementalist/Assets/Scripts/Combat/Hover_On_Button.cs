using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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

            Combat_Manager manager = GameObject.FindGameObjectWithTag("CombatManager").GetComponent<Combat_Manager>();

            float multiplier = ElementManager.Instance.GetDamageMultiplier((selectedElement.GetName(),
                manager.spawnedEnemies[0].GetComponent<Grunt_Combat>().element.GetName()));

            if (multiplier == 2f)
            {
                HUD.DialogueText.text += "\nSuper Effective";
            }
            else if (multiplier == 0.5f)
            {
                HUD.DialogueText.text += "\nNot Very Effective";
            }
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
