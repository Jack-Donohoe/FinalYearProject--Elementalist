using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private GameObject otherTeleporter;
    private bool nearDoor;

    private void Start()
    {
        if (gameObject.name == "Teleporter 1")
        {
            otherTeleporter = transform.parent.Find("Teleporter 2").gameObject;
        }
        else if (gameObject.name == "Teleporter 2")
        {
            otherTeleporter = transform.parent.Find("Teleporter 1").gameObject;
        }
    }

    private void Update()
    {
        HandleDoor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            nearDoor = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            nearDoor = false;
        }
    }

    void HandleDoor()
    {
        if (nearDoor && Input.GetKeyDown(KeyCode.E))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = new Vector3(otherTeleporter.transform.position.x, 1.5f,
                otherTeleporter.transform.position.z);
            player.GetComponent<CharacterController>().enabled = true;
        }
    }
}
