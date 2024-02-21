using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level_Room : MonoBehaviour
{
    public GameObject enemy;

    public GameObject[] waypoints;

    private GameObject createdEnemy;

    public void SpawnEnemy()
    {
        createdEnemy = Instantiate(enemy, transform.position, Quaternion.identity, this.transform);
        createdEnemy.GetComponent<Enemy_Exploration>().SetWaypoints(waypoints);
        createdEnemy.GetComponent<Enemy_Exploration>().Setup();
    }
}
