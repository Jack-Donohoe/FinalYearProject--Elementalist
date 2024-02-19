using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Exploration : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Enemy"))
        {
            GameManager.instance.StartCombat();
            Destroy(collider.gameObject,1);
        }
    }

    public void OnInventory()
    {
        GameObject.FindGameObjectWithTag("HUD").GetComponent<Exploration_HUD>().HandleInventoryMenu();
    }
}
