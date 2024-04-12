using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Exploration : MonoBehaviour
{
    private Exploration_HUD hud;
    
    // float timer = 0f;

    private void Start()
    {
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<Exploration_HUD>();
        hud.UpdateStatsMenu();
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            hud.HandlePauseMenu();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            hud.HandleInventoryMenu();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Enemy"))
        {
            GameObject.FindGameObjectWithTag("Map").GetComponent<ProcGenV4>().SetRoomCompleted(collider.gameObject.transform.parent.transform.position, true);
            Debug.Log(collider.gameObject.GetComponent<Enemy_Exploration>().enemyElement);

            Enemy_Exploration enemy = collider.gameObject.GetComponent<Enemy_Exploration>();
            GameManager.Instance.StartCombat(enemy.enemyElement, enemy.eliteEnemy);
        }
    }
}
