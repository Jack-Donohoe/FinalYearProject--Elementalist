using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Exploration : MonoBehaviour
{
    public enum State
    {
        Patrol,
        PlayerDetected,
        Hunt
    }

    public State enemyState;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyState = State.Patrol;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
