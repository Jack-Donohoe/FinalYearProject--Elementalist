using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Exploration : MonoBehaviour
{
    private Exploration_HUD hud;

    private (int, int, int, int) playerInfo;
    private int health;
    private int magic;
    
    float timer = 0f;

    private void Start()
    {
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<Exploration_HUD>();
        playerInfo = GameManager.Instance.GetPlayerInfo();
        health = playerInfo.Item1;
        magic = playerInfo.Item2;
        hud.setPlayerHP(health);
        hud.setMP(magic);
    }
    
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > 10)
        {
            timer = 0;
            playerInfo = GameManager.Instance.GetPlayerInfo();
            health = playerInfo.Item1 + 5;
            
            if (health > 100) 
            { 
                health = 100;
            }
             
            magic = playerInfo.Item2 + 5;
             
            if (magic > 50)
            { 
                magic = 50;
            }
             
            GameManager.Instance.SetPlayerInfo((health,magic,playerInfo.Item3,playerInfo.Item4));
            hud.setPlayerHP(health); 
            hud.setMP(magic);
            Debug.Log("Health: " + health + " Magic: " + magic);
        }
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
