using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Exploration : MonoBehaviour
{
    public GameManager manager;
    
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Enemy"))
        {
            manager.StartCombat();
        }
    }
}
