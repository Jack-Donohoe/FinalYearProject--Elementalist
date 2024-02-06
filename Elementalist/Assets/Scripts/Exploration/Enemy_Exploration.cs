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
        PlayerDetected
    }

    public State enemyState;

    private Transform target;
    public Transform[] waypoints;

    private int waypointCount;
    
    private NavMeshAgent enemyAgent;
    
    GameObject player;
    private Transform lastPlayerPos;

    private float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        int rand = Random.Range(0,waypoints.Length);
        transform.position = waypoints[rand].position;

        waypointCount = rand + 1;
        if (waypointCount >= waypoints.Length)
        {
            waypointCount = 0;
        }
        
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
                    waypointCount = Random.Range(0, waypoints.Length);
                    //if (waypointCount > waypoints.Length - 1) waypointCount = 0;
                }

                enemyAgent.SetDestination(target.position);
                break;
            }

            case State.PlayerDetected:
            {
                target = player.transform;

                NavMeshPath path = new NavMeshPath();
                enemyAgent.CalculatePath(target.position, path);

                if (path.status != NavMeshPathStatus.PathInvalid)
                {
                    enemyAgent.SetDestination(target.position);
                }
                else
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
}
