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
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            hud.HandlePauseMenu();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            hud.HandleInventoryMenu();
        }

        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        bool nearDoor = false;

        foreach (GameObject door in doors)
        {
            if (Vector3.Distance(transform.position, door.transform.position) < 2f)
            {
                nearDoor = true;
            }
        }

        hud.interactText.gameObject.SetActive(nearDoor);
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
