using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level_Room : MonoBehaviour
{
    public GameObject[] enemyTypes;

    public GameObject[] waypoints;

    private GameObject createdEnemy;

    public void SpawnEnemy(int enemyNum, bool eliteEnemy)
    {
        if (!eliteEnemy)
        {
            createdEnemy = Instantiate(enemyTypes[enemyNum], transform.position, Quaternion.identity, transform);
            createdEnemy.GetComponent<Enemy_Exploration>().SetWaypoints(waypoints);
            createdEnemy.GetComponent<Enemy_Exploration>().Setup();
        }
        else
        {
            createdEnemy = Instantiate(enemyTypes[enemyNum], new Vector3(transform.position.x, 1.5f, transform.position.z + 15f), Quaternion.Euler(0,180,0), transform);
        }
    }
}
