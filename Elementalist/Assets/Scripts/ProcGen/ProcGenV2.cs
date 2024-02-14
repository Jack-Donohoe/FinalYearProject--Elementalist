using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcGenV2 : MonoBehaviour
{
    public int levelSize;

    public GameObject startRoom, endRoom;
    public GameObject[] emptyRooms;
    public GameObject[] enemyRooms;

    public double enemyFrequency;

    private int[,] level;

    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        level = new int[levelSize, levelSize];

        GenerateMap();

        GameObject.Find("Player").transform.position = startPos;
    }

    void GenerateMap()
    {
        int rand_position = Random.Range(1, levelSize - 2);

        level[levelSize - 1, rand_position] = 3;

        rand_position = Random.Range(1, levelSize - 2);

        level[0, rand_position] = 4;

        for (int x = 0; x < levelSize; x++)
        {
            for (int z = 0; z < levelSize; z++)
            {
                if (level[x, z] == 0)
                {
                    double rand = Random.Range(0f, 1.0f);

                    if (rand < enemyFrequency)
                    {
                        level[x, z] = 2;
                    }
                    else
                    {
                        level[x, z] = 1;
                    }
                }
            }
        }

        for (int x = 0; x < levelSize; x++)
        {
            for (int z = 0; z < levelSize; z++)
            {
                if (level[x, z] == 1)
                {
                    if (x == 0 && z == 0)
                    {
                        GameObject room = (Instantiate(emptyRooms[2], new Vector3(x * 20f, 0f, z * 20f),
                            Quaternion.identity, this.transform));
                        room.transform.Rotate(0f, 90f, 0f);
                    }
                    else if (x == 0 && z == levelSize - 1)
                    {
                        GameObject room = (Instantiate(emptyRooms[2], new Vector3(x * 20f, 0f, z * 20f),
                            Quaternion.identity, this.transform));
                        room.transform.Rotate(0f, 180f, 0f);
                    }
                    else if (x == levelSize - 1 && z == 0)
                    {
                        GameObject room = (Instantiate(emptyRooms[2], new Vector3(x * 20f, 0f, z * 20f),
                            Quaternion.identity, this.transform));
                    }
                    else if (x == levelSize - 1 && z == levelSize - 1)
                    {
                        GameObject room = (Instantiate(emptyRooms[2], new Vector3(x * 20f, 0f, z * 20f),
                            Quaternion.identity, this.transform));
                        room.transform.Rotate(0f, -90f, 0f);
                    }
                    else if (x == 0)
                    {
                        GameObject room = (Instantiate(emptyRooms[1], new Vector3(x * 20f, 0f, z * 20f),
                            Quaternion.identity, this.transform));
                        room.transform.Rotate(0f, 90f, 0f);
                    }
                    else if (x == levelSize - 1)
                    {
                        GameObject room = (Instantiate(emptyRooms[1], new Vector3(x * 20f, 0f, z * 20f),
                            Quaternion.identity, this.transform));
                        room.transform.Rotate(0f, -90f, 0f);
                    }
                    else if (z == 0)
                    {
                        GameObject room = (Instantiate(emptyRooms[1], new Vector3(x * 20f, 0f, z * 20f),
                            Quaternion.identity, this.transform));
                    }
                    else if (z == levelSize - 1)
                    {
                        GameObject room = (Instantiate(emptyRooms[1], new Vector3(x * 20f, 0f, z * 20f),
                            Quaternion.identity, this.transform));
                        room.transform.Rotate(0f, 180f, 0f);
                    }
                    else
                    {
                        GameObject room = (Instantiate(emptyRooms[0], new Vector3(x * 20f, 0f, z * 20f),
                            Quaternion.identity, this.transform));
                    }
                }
                else if (level[x, z] == 2)
                {
                    if (x == 0 && z == 0)
                    {
                        GameObject room = (Instantiate(enemyRooms[2], new Vector3(x * 20f, 0f, z * 20f),
                            Quaternion.identity, this.transform));
                        room.transform.Rotate(0f, 90f, 0f);
                    }
                    else if (x == 0 && z == levelSize - 1)
                    {
                        GameObject room = (Instantiate(enemyRooms[2], new Vector3(x * 20f, 0f, z * 20f),
                            Quaternion.identity, this.transform));
                        room.transform.Rotate(0f, 180f, 0f);
                    }
                    else if (x == levelSize - 1 && z == 0)
                    {
                        GameObject room = (Instantiate(enemyRooms[2], new Vector3(x * 20f, 0f, z * 20f),
                            Quaternion.identity, this.transform));
                    }
                    else if (x == levelSize - 1 && z == levelSize - 1)
                    {
                        GameObject room = (Instantiate(enemyRooms[2], new Vector3(x * 20f, 0f, z * 20f),
                            Quaternion.identity, this.transform));
                        room.transform.Rotate(0f, -90f, 0f);
                    }
                    else if (x == 0)
                    {
                        GameObject room = (Instantiate(enemyRooms[1], new Vector3(x * 20f, 0f, z * 20f),
                            Quaternion.identity, this.transform));
                        room.transform.Rotate(0f, 90f, 0f);
                    }
                    else if (x == levelSize - 1)
                    {
                        GameObject room = (Instantiate(enemyRooms[1], new Vector3(x * 20f, 0f, z * 20f),
                            Quaternion.identity, this.transform));
                        room.transform.Rotate(0f, -90f, 0f);
                    }
                    else if (z == 0)
                    {
                        GameObject room = (Instantiate(enemyRooms[1], new Vector3(x * 20f, 0f, z * 20f),
                            Quaternion.identity, this.transform));
                    }
                    else if (z == levelSize - 1)
                    {
                        GameObject room = (Instantiate(enemyRooms[1], new Vector3(x * 20f, 0f, z * 20f),
                            Quaternion.identity, this.transform));
                        room.transform.Rotate(0f, 180f, 0f);
                    }
                    else
                    {
                        GameObject room = (Instantiate(enemyRooms[0], new Vector3(x * 20f, 0f, z * 20f),
                            Quaternion.identity, this.transform));
                    }
                } 
                else if (level[x, z] == 3)
                {
                    GameObject room = (Instantiate(startRoom, new Vector3(x * 20f, 0f, z * 20f),
                        Quaternion.identity, this.transform));
                    room.transform.Rotate(0f, -90f, 0f);
                    
                    startPos = new Vector3(room.transform.position.x, 1.5f, room.transform.position.z);
                }
                else if (level[x, z] == 4)
                {
                    GameObject room = (Instantiate(endRoom, new Vector3(x * 20f, 0f, z * 20f),
                        Quaternion.identity, this.transform));
                    room.transform.Rotate(0f, 90f, 0f);
                }
            }
        }
    }
}