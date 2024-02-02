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
        Hunt
    }

    public State enemyState;

    private Transform target;
    public Transform[] waypoints;

    private int waypointCount = 0;
    
    private NavMeshAgent enemyAgent;
    
    GameObject player;
    private Transform lastPlayerPos;

    private float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyState = State.Patrol;
        enemyAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        switch (enemyState)
        {
            case State.Patrol:
            {
                target = waypoints[waypointCount];
                
                if (Vector3.Distance(transform.position, target.position) < 1.5)
                {
                    waypointCount++;
                    if (waypointCount > waypoints.Length - 1) waypointCount = 0;
                }

                enemyAgent.SetDestination(target.position);
                break;
            }

            case State.PlayerDetected:
            {
                target = player.transform;

                enemyAgent.SetDestination(target.position);
                break;
            }

            case State.Hunt:
            {
                target = lastPlayerPos.transform;
                if (timer % 4 == 0)
                {
                    enemyState = State.Patrol;
                }
                break;
            }
        }
        
        LookForPlayer();
        enemyAgent.SetDestination(target.position);
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

        float castDistance = 20;
        ray.direction = transform.forward * castDistance;
        Debug.DrawRay(ray.origin, ray.direction * castDistance, Color.red);

        if (Physics.Raycast(ray.origin, lookDirection, out hit, castDistance))
        {
            if (seenObject == "Player" || inFOV)
            {
                enemyState = State.PlayerDetected;
            }
            else
            {
                lastPlayerPos = player.transform;
                enemyState = State.Hunt;
            }
        }
    }
}
