using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Exploration : MonoBehaviour
{
    private Exploration_HUD hud;

    private int max_health;
    private int health;
    private int max_magic;
    private int magic;
    
    // float timer = 0f;

    private void Start()
    {
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<Exploration_HUD>();
        max_health = GameManager.Instance.Max_Health;
        health = GameManager.Instance.HP;
        max_magic = GameManager.Instance.Max_Magic;
        magic = GameManager.Instance.MP;
        hud.setPlayerHP(health, max_health);
        hud.setMP(magic, max_magic);
    }
    
    private void Update()
    {
        // timer += Time.deltaTime;
        //
        // if (timer > 6)
        // {
        //     timer = 0;
        //     health = GameManager.Instance.HP + 5;
        //     
        //     if (health > GameManager.Instance.Max_Health) 
        //     { 
        //         health = GameManager.Instance.Max_Health;
        //     }
        //      
        //     magic = GameManager.Instance.MP + 5;
        //      
        //     if (magic > GameManager.Instance.Max_Magic)
        //     { 
        //         magic = GameManager.Instance.Max_Magic;
        //     }
        //      
        //     GameManager.Instance.SetPlayerResources(health, magic);
        //     hud.setPlayerHP(health); 
        //     hud.setMP(magic);
        //     Debug.Log("Health: " + health + " Magic: " + magic);
        // }
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
