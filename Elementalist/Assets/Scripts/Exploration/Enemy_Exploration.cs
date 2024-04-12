using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Exploration : MonoBehaviour
{
    public enum State
    {
        Patrol,
        PlayerDetected,
        Guardian
    }

    public State enemyState;

    private Transform target;
    private GameObject[] waypoints;

    private int waypointCount;
    
    private NavMeshAgent enemyAgent;
    
    GameObject player;
    private Transform lastPlayerPos;

    public Element enemyElement;

    public bool eliteEnemy;

    // Update is called once per frame
    void Update()
    {
        switch (enemyState)
        {
            case State.Patrol:
            {
                target = waypoints[waypointCount].transform;
                
                if (Vector3.Distance(transform.position, target.position) < 1.5)
                {
                    waypointCount = Random.Range(0, waypoints.Length);
                }
                
                break;
            }

            case State.PlayerDetected:
            {
                target = player.transform;
                break;
            }
            
            case State.Guardian:
                GetComponent<NavMeshAgent>().enabled = false;
                break;
        }

        if (!eliteEnemy)
        {
            LookForPlayer();
            DetectPlayerNearby();
            enemyAgent.SetDestination(target.position);
        }
    }
    
    public void Setup()
    {
        int rand = Random.Range(0,waypoints.Length);
        transform.position = waypoints[rand].transform.position;

        waypointCount = rand + 1;
        if (waypointCount >= waypoints.Length)
        {
            waypointCount = 0;
        }
        
        enemyState = State.Patrol;
        enemyAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void LookForPlayer()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        
        bool inFOV = (Vector3.Dot(transform.forward.normalized, lookDirection) > 0.7f);
        
        Debug.DrawRay(transform.position, lookDirection * 100, Color.green);
        Debug.DrawRay(transform.position, transform.forward * 100, Color.cyan);
        
        Debug.DrawRay(transform.position, (transform.forward - transform.right) * 100, Color.red);
        Debug.DrawRay(transform.position, (transform.forward + transform.right) * 100, Color.red);

        Ray ray = new Ray();
        RaycastHit hit;
        
        ray.origin = transform.position + Vector3.up * 0.7f;
        string seenObject = "";

        float castDistance = 10;
        ray.direction = transform.forward * castDistance;
        Debug.DrawRay(ray.origin, ray.direction * castDistance, Color.red);

        if (Physics.Raycast(ray.origin, lookDirection, out hit, castDistance))
        {
            seenObject = hit.collider.gameObject.name;
            
            if (seenObject == "Player" || inFOV)    
            {
                enemyState = State.PlayerDetected;
            }
            else
            {
                enemyState = State.Patrol;
            }
        }
    }

    void DetectPlayerNearby()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < 5)
        {
            enemyState = State.PlayerDetected;
        } 
        else
        {
            enemyState = State.Patrol;
        }
    }

    public void SetWaypoints(GameObject[] waypoints)
    {
        this.waypoints = waypoints;
    }
}
