using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level_Room : MonoBehaviour
{
    public GameObject[] enemyTypes;

    public GameObject[] waypoints;

    private GameObject createdEnemy;

    public void SpawnEnemy(int enemyNum)
    {
        createdEnemy = Instantiate(enemyTypes[enemyNum], transform.position, Quaternion.identity, transform);
        createdEnemy.GetComponent<Enemy_Exploration>().SetWaypoints(waypoints);
        createdEnemy.GetComponent<Enemy_Exploration>().Setup();
    }
}
