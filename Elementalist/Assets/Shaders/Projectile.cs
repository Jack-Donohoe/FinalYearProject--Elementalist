using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 move_vector;
    private float timer = 0f;
    
    private void Update()
    {
        transform.position += move_vector;
        
        timer += Time.deltaTime;
        if (timer > 10)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    public void SetMoveDirection(Vector3 move_dir)
    {
        move_vector = move_dir;
    }
}
