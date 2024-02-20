using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProcGenV2 : MonoBehaviour
{
    public int levelSize;

    public GameObject startRoom, endRoom;
    public GameObject[] emptyRooms;
    public GameObject[] enemyRooms;

    [Range(0,1)]
    public double enemyFrequency;

    private int[,] level;
    
    private Vector3 startPos;

    public void OnLevelLoad()
    {
        level = CreateMap();
        GenerateMap(level);

        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").transform.position = startPos;
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().enabled = true;
        Debug.Log(GameObject.FindGameObjectWithTag("Player").transform.position);    
    }

    private int[,] CreateMap()
    {
        int[,] _level = new int[levelSize, levelSize];
        
        int rand_position = Random.Range(1, levelSize - 2);

        _level[levelSize - 1, rand_position] = 3;

        rand_position = Random.Range(1, levelSize - 2);

        _level[0, rand_position] = 4;

        for (int x = 0; x < levelSize; x++)
        {
            for (int z = 0; z < levelSize; z++)
            {
                if (_level[x, z] == 0)
                {
                    double rand = Random.Range(0f, 1.0f);

                    if (rand < enemyFrequency)
                    {
                        _level[x, z] = 2;
                    }
                    else
                    {
                        _level[x, z] = 1;
                    }
                }
            }
        }
        
        return _level;
    }
    
    public void GenerateMap(int[,] _level)
    {
        level = _level;
        for (int x = 0; x < levelSize; x++)
        {
            for (int z = 0; z < levelSize; z++)
            {
                if (_level[x, z] == 1)
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
                        int rand = Random.Range(0, 10);
                        if (rand >= 8)
                        {
                            GameObject room = (Instantiate(emptyRooms[0], new Vector3(x * 20f, 0f, z * 20f),
                                Quaternion.identity, this.transform));
                        }
                        else if (rand is < 8 and >= 5)
                        {
                            GameObject room = (Instantiate(emptyRooms[2], new Vector3(x * 20f, 0f, z * 20f),
                                Quaternion.identity, this.transform));
                            
                            int rand_angle = Random.Range(0, 4);
                            room.transform.Rotate(0f, 90f * rand_angle, 0f);
                        }
                        else
                        {
                            GameObject room = (Instantiate(emptyRooms[1], new Vector3(x * 20f, 0f, z * 20f),
                                Quaternion.identity, this.transform));
                            
                            int rand_angle = Random.Range(0, 4);
                            room.transform.Rotate(0f, 90f * rand_angle, 0f);
                        }
                    }
                }
                else if (_level[x, z] == 2)
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
                        int rand = Random.Range(0, 10);
                        if (rand >= 8)
                        {
                            GameObject room = (Instantiate(enemyRooms[0], new Vector3(x * 20f, 0f, z * 20f),
                                Quaternion.identity, this.transform));
                        }
                        else if (rand is < 8 and >= 5)
                        {
                            GameObject room = (Instantiate(enemyRooms[2], new Vector3(x * 20f, 0f, z * 20f),
                                Quaternion.identity, this.transform));
                            
                            int rand_angle = Random.Range(0, 4);
                            room.transform.Rotate(0f, 90f * rand_angle, 0f);
                        }
                        else
                        {
                            GameObject room = (Instantiate(enemyRooms[1], new Vector3(x * 20f, 0f, z * 20f),
                                Quaternion.identity, this.transform));
                            
                            int rand_angle = Random.Range(0, 4);
                            room.transform.Rotate(0f, 90f * rand_angle, 0f);
                        }
                    }
                } 
                else if (_level[x, z] == 3)
                {
                    GameObject room = (Instantiate(startRoom, new Vector3(x * 20f, 0f, z * 20f),
                        Quaternion.identity, this.transform));
                    room.transform.Rotate(0f, -90f, 0f);
                    
                    startPos = new Vector3(room.transform.position.x, 1.5f, room.transform.position.z);
                }
                else if (_level[x, z] == 4)
                {
                    GameObject room = (Instantiate(endRoom, new Vector3(x * 20f, 0f, z * 20f),
                        Quaternion.identity, this.transform));
                    room.transform.Rotate(0f, 90f, 0f);
                }
            }
        }
    }

    public int[] GetLevel()
    {
        int[] newLevel = new int[levelSize * levelSize];

        for (int x = 0; x < levelSize; x++)
        {
            for (int z = 0; z < levelSize; z++)
            {
                newLevel[x * levelSize + z] = level[x, z];
            }
        }

        return newLevel;
    }
}