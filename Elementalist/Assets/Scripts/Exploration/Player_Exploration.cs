using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Exploration : MonoBehaviour
{
    private Exploration_HUD hud;

    private void Start()
    {
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<Exploration_HUD>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Enemy"))
        {
            GameObject.FindGameObjectWithTag("Map").GetComponent<ProcGenV3>().SetRoomCompleted(collider.gameObject.transform.parent.transform.position, true);
            GameManager.Instance.StartCombat();
        }
    }

    public void OnPause()
    {
        hud.HandlePauseMenu();
    }
    
    public void OnInventory()
    {
        hud.HandleInventoryMenu();
    }
}
