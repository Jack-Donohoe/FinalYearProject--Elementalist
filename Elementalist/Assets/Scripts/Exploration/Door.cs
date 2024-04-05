using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 2f)
        {
            Debug.Log("Near Door");
            HandleDoor();
        }
    }

    void HandleDoor()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = new Vector3(player.transform.position.x + player.transform.forward.normalized.x, 0.25f, player.transform.position.z + player.transform.forward.normalized.z);
            player.GetComponent<CharacterController>().enabled = true;
        }
    }
}
