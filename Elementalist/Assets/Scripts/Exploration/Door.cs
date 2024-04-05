using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject warpPoint;
    private bool nearDoor;

    private void Start()
    {
        // if (gameObject.name == "Teleporter 1")
        // {
        //     warpPoint = transform.parent.Find("WarpPoint 2").gameObject;
        // }
        // else if (gameObject.name == "Teleporter 2")
        // {
        //     warpPoint = transform.parent.Find("WarpPoint 1").gameObject;
        // }
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
            player.transform.position = new Vector3(warpPoint.transform.position.x, 0.25f, warpPoint.transform.position.z);
            player.GetComponent<CharacterController>().enabled = true;
        }
    }
}
