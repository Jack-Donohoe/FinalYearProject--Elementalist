using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProcGenV4 : MonoBehaviour
{
    public RoomV2[,] level;
    public int levelSize;
    
    public GameObject[] roomTemplates;

    public Dictionary<(bool, bool, bool, bool), (GameObject, float)> rooms;
    
    public Vector3 startPos { get; set; }

    public GameObject door;

    [Range(0,1)]
    public float fireEnemyFrequency = 0f;
    
    [Range(0,1)]
    public float waterEnemyFrequency = 0f;
    
    [Range(0,1)]
    public float earthEnemyFrequency = 0f;
    
    [Range(0,1)]
    public float airEnemyFrequency = 0f;
    
    void Awake()
    {
        rooms = new Dictionary<(bool, bool, bool, bool), (GameObject, float)>()
        {
            // One Entrance Rooms
            { (true, false, false, false), (roomTemplates[0], 0f) },
            { (false, true, false, false), (roomTemplates[0], 90f) },
            { (false, false, true, false), (roomTemplates[0], 180f) },
            { (false, false, false, true), (roomTemplates[0], -90f) },
            // Corner Rooms
            { (true, true, false, false), (roomTemplates[1], -90f) },
            { (true, false, false, true), (roomTemplates[1], 180f) },
            { (false, true, true, false), (roomTemplates[1], 0f) },
            { (false, false, true, true), (roomTemplates[1], 90f) },
            // Straight Rooms
            { (true, false, true, false), (roomTemplates[2], 0f) },
            { (false, true, false, true), (roomTemplates[2], 90f) },
            // T Junction Rooms
            { (true, true, false, true), (roomTemplates[3], 180f) },
            { (true, true, true, false), (roomTemplates[3], -90f) },
            { (false, true, true, true), (roomTemplates[3], 0f) },
            { (true, false, true, true), (roomTemplates[3], 90f) },
            // Four Entrance Room
            { (true, true, true, true), (roomTemplates[4], 0f) }
        };
    }

    // Used for testing grid generation
    // private void Start()
    // {
    //     GenerateLevel(PopulateMap());
    // }

    public void OnLevelLoad()
    {
        GenerateLevel(PopulateMap());
    }

    private RoomV2[,] PopulateMap()
    {
        RoomV2[,] currentLevel = new RoomV2[levelSize, levelSize];
        
        // Initial populate of array, random connections
        for (int i = 0; i < levelSize; i++)
        {
            for (int j = 0; j < levelSize; j++)
            {
                RoomV2 room = new RoomV2();
                
                room.completed = false;
        
                room.roomType = Random.Range(0, 10) > 5 ? RoomV2.RoomType.Empty : RoomV2.RoomType.Enemy;
        
                room.connections["top"] = Random.Range(0, 10) <= 6;
                room.connections["left"] = Random.Range(0, 10) <= 5;
                room.connections["bottom"] = Random.Range(0, 10) <= 5;
                room.connections["right"] = Random.Range(0, 10) <= 5;
        
                currentLevel[i, j] = room;
            }
        }
        
        // Process Top Left Corner
        currentLevel[0, 0].SetAllConnections((false,false,true,true));
        
        // Process Top Right Corner
        currentLevel[0, levelSize - 1].SetAllConnections((false,true,true,false));
        
        // Process Bottom Left Corner
        currentLevel[levelSize - 1, 0].SetAllConnections((true,false,false,true));
        
        // Process Bottom Right Corner
        currentLevel[levelSize - 1, levelSize - 1].SetAllConnections((true,true,false,false));
        
        // Process Edges
        for (int i = 1; i < levelSize - 1; i++)
        {
            // Handle Top Edge
            currentLevel[0, i].connections["top"] = false;
            if (currentLevel[0, i - 1].connections["right"])
            {
                currentLevel[0, i].connections["left"] = true;
            }
            else
            {
                currentLevel[0, i].connections["left"] = false;
            }
            
            if (currentLevel[1, i - 1].connections["top"])
            {
                currentLevel[0, i].connections["bottom"] = true;
            }
            else
            {
                currentLevel[0, i].connections["bottom"] = false;
            }
            
            // Handle Left Edge
            currentLevel[i, 0].connections["left"] = false;
            if (currentLevel[i - 1, 0].connections["bottom"])
            {
                currentLevel[i, 0].connections["top"] = true;
            }
            else
            {
                currentLevel[i, 0].connections["top"] = false;
            }
            
            if (currentLevel[i - 1, 1].connections["left"])
            {
                currentLevel[i, 0].connections["right"] = true;
            }
            else
            {
                currentLevel[i, 0].connections["right"] = false;
            }
            
            // Handle Bottom Edge
            currentLevel[levelSize - 1, i].connections["bottom"] = false;
            if (currentLevel[levelSize - 1, i - 1].connections["right"])
            {
                currentLevel[levelSize - 1, i].connections["left"] = true;
            }
            else
            {
                currentLevel[levelSize - 1, i].connections["left"] = false;
            }
            
            if (currentLevel[levelSize - 2, i].connections["bottom"])
            {
                currentLevel[levelSize - 1, i].connections["top"] = true;
            }
            else
            {
                currentLevel[levelSize - 1, i].connections["top"] = false;
            }
            
            // Handle Right Edge
            currentLevel[i, levelSize - 1].connections["right"] = false;
            if (currentLevel[i - 1, levelSize - 1].connections["bottom"])
            {
                currentLevel[i, levelSize - 1].connections["top"] = true;
            }
            else
            {
                currentLevel[i, levelSize - 1].connections["top"] = false;
            }
            
            if (currentLevel[i, levelSize - 2].connections["right"])
            {
                currentLevel[i, levelSize - 1].connections["left"] = true;
            }
            else
            {
                currentLevel[i, levelSize - 1].connections["left"] = false;
            }
        }
        
        // Process inner grid
        for (int i = 1; i < levelSize - 1; i++)
        {
            for (int j = 1; j < levelSize - 1; j++)
            {
                if (currentLevel[i, j - 1].connections["right"])
                {
                    currentLevel[i, j].connections["left"] = true;
                }
                else
                {
                    currentLevel[i, j].connections["left"] = false;
                }
        
                if (currentLevel[i - 1, j].connections["bottom"])
                {
                    currentLevel[i, j].connections["top"] = true;
                }
                else
                {
                    currentLevel[i, j].connections["top"] = false;
                }
            }
        }
        
        // Check connections on corners once array has been fully implemented
        // Top Left Corner
        if (currentLevel[0, 1].connections["left"])
        {
            currentLevel[0, 0].connections["right"] = true;
        }
        else
        {
            currentLevel[0, 0].connections["right"] = false;
        }

        if (currentLevel[1, 0].connections["top"])
        {
            currentLevel[0, 0].connections["bottom"] = true;
        }
        else
        {
            currentLevel[0, 0].connections["bottom"] = false;
        }
        
        // Top Right Corner
        if (currentLevel[0, levelSize - 2].connections["right"])
        {
            currentLevel[0, levelSize - 1].connections["left"] = true;
        }
        else
        {
            currentLevel[0, levelSize - 1].connections["left"] = false;
        }
        
        if (currentLevel[1, levelSize - 1].connections["top"])
        {
            currentLevel[0, levelSize - 1].connections["bottom"] = true;
        }
        else
        {
            currentLevel[0, levelSize - 1].connections["bottom"] = false;
        }
        
        // Bottom Left Corner
        if (currentLevel[levelSize - 1, 1].connections["left"])
        {
            currentLevel[levelSize - 1, 0].connections["right"] = true;
        }
        else
        {
            currentLevel[levelSize - 1, 0].connections["right"] = false;
        }

        if (currentLevel[levelSize - 2, 0].connections["bottom"])
        {
            currentLevel[levelSize - 1, 0].connections["top"] = true;
        }
        else
        {
            currentLevel[levelSize - 1, 0].connections["top"] = false;
        }
        
        // Bottom Right Corner
        if (currentLevel[levelSize - 1, levelSize - 2].connections["right"])
        {
            currentLevel[levelSize - 1, levelSize - 1].connections["left"] = true;
        }
        else
        {
            currentLevel[levelSize - 1, levelSize - 1].connections["left"] = false;
        }

        if (currentLevel[levelSize - 2, levelSize - 1].connections["bottom"])
        {
            currentLevel[levelSize - 1, levelSize - 1].connections["top"] = true;
        }
        else
        {
            currentLevel[levelSize - 1, levelSize - 1].connections["top"] = false;
        }

        // Select Start Room in top row
        int rand = Random.Range(1, levelSize - 2);
        currentLevel[0, rand].roomType = RoomV2.RoomType.Start;
        currentLevel[0, rand].SetAllConnections((true, true, false, true));
        currentLevel[1, rand].connections["top"] = true;
        currentLevel[0, rand - 1].connections["right"] = true;
        currentLevel[0, rand + 1].connections["left"] = true;
        
        // Select End Room in bottom row
        rand = Random.Range(1, levelSize - 2);
        currentLevel[levelSize - 1, rand].roomType = RoomV2.RoomType.End;
        currentLevel[levelSize - 1, rand].SetAllConnections((false, true, true, true));
        currentLevel[levelSize - 2, rand].connections["bottom"] = true;
        currentLevel[levelSize - 1, rand - 1].connections["right"] = true;
        currentLevel[levelSize - 1, rand + 1].connections["left"] = true;

        return currentLevel;
    }

    public void GenerateLevel(RoomV2[,] levelToGenerate)
    {
        Debug.Log("Generating Level...");
        level = levelToGenerate;
        for (int i = 0; i < levelSize; i++)
        {
            for (int j = 0; j < levelSize; j++)
            {
                (bool, bool, bool, bool) connections = levelToGenerate[i, j].GetAllConnections();

                if (levelToGenerate[i, j].roomType == RoomV2.RoomType.Empty || levelToGenerate[i, j].roomType == RoomV2.RoomType.Enemy || levelToGenerate[i, j].roomType == RoomV2.RoomType.Reward)
                {
                    if (rooms.TryGetValue(connections, out (GameObject, float) roomInfo))
                    {
                        GameObject room = Instantiate(roomInfo.Item1, new Vector3(j * 35f, 0f, i * 35f),
                            Quaternion.identity, this.transform);
                        room.name = "Room" + j + " " + i;
                        room.transform.Rotate(0f, roomInfo.Item2, 0f);

                        if (levelToGenerate[i, j].roomType == RoomV2.RoomType.Enemy)
                        {
                            if (levelToGenerate[i, j].completed == false)
                            {
                                int enemyNum = Random.value < fireEnemyFrequency ? 0 : 1;
                                
                                room.GetComponent<Level_Room>().SpawnEnemy(enemyNum, false);
                            }
                        }
                        else
                        {
                            room.GetComponent<NavMeshSurface>().enabled = false;
                        }

                        if (connections.Item1)
                        {
                            Instantiate(door, new Vector3(room.transform.position.x, 2f, room.transform.position.z - 17.5f), Quaternion.Euler(new Vector3(0f, 90f, 0f)),
                                room.transform);
                        }
                        
                        if (connections.Item2)
                        {
                            Instantiate(door, new Vector3(room.transform.position.x - 17.5f, 2f, room.transform.position.z), Quaternion.identity,
                                room.transform);
                        }
                        
                        // if (connections.Item3)
                        // {
                        //     Instantiate(door, new Vector3(room.transform.position.x, 2f, room.transform.position.z + 17.5f), Quaternion.Euler(new Vector3(0f, 90f, 0f)),
                        //         room.transform);
                        // }
                        //
                        // if (connections.Item4)
                        // {
                        //     Instantiate(door, new Vector3(room.transform.position.x + 17.5f, 2f, room.transform.position.z), Quaternion.identity,
                        //         room.transform);
                        // }
                    }
                }
                else if (levelToGenerate[i, j].roomType == RoomV2.RoomType.Start)
                {
                    startPos = new Vector3(j * 35f, 0.25f, i * 35f);
                    GameObject room = Instantiate(roomTemplates[5], new Vector3(j * 35f, 0f, i * 35f),
                        Quaternion.identity, this.transform);
                    room.name = "Room" + j + " " + i;
                    room.transform.Rotate(0f, 0f, 0f);
                    
                    if (connections.Item1)
                    {
                        Instantiate(door, new Vector3(room.transform.position.x, 2f, room.transform.position.z + 17.5f), Quaternion.Euler(new Vector3(0f, 90f, 0f)),
                            room.transform);
                    }
                        
                    if (connections.Item2)
                    {
                        Instantiate(door, new Vector3(room.transform.position.x + 17.5f, 2f, room.transform.position.z), Quaternion.identity,
                            room.transform);
                    }
                        
                    // if (connections.Item3)
                    // {
                    //     Instantiate(door, new Vector3(room.transform.position.x, 2f, room.transform.position.z - 17.5f), Quaternion.Euler(new Vector3(0f, 90f, 0f)),
                    //         room.transform);
                    // }
    
                    if (connections.Item4)
                    {
                        Instantiate(door, new Vector3(room.transform.position.x - 17.5f, 2f, room.transform.position.z), Quaternion.identity,
                            room.transform);
                    }
                }
                else if (levelToGenerate[i, j].roomType == RoomV2.RoomType.End)
                {
                    GameObject room = Instantiate(roomTemplates[6], new Vector3(j * 35f, 0f, i * 35f),
                        Quaternion.identity, this.transform);
                    room.name = "Room" + j + " " + i;
                    room.transform.Rotate(0f, 180f, 0f);
                    
                    if (levelToGenerate[i, j].completed == false)
                    {
                        int enemyNum = Random.value < 0.7 ? 0 : 1;
                                
                        room.GetComponent<Level_Room>().SpawnEnemy(enemyNum, true);
                    }
                    
                    // if (connections.Item1)
                    // {
                    //     Instantiate(door, new Vector3(room.transform.position.x, 2f, room.transform.position.z + 17.5f), Quaternion.Euler(new Vector3(0f, 90f, 0f)),
                    //         room.transform);
                    // }
                        
                    if (connections.Item2)
                    {
                        Instantiate(door, new Vector3(room.transform.position.x + 17.5f, 2f, room.transform.position.z), Quaternion.identity,
                            room.transform);
                    }
                        
                    if (connections.Item3)
                    {
                        Instantiate(door, new Vector3(room.transform.position.x, 2f, room.transform.position.z - 17.5f), Quaternion.Euler(new Vector3(0f, 90f, 0f)),
                            room.transform);
                    }
                        
                    if (connections.Item4)
                    {
                        Instantiate(door, new Vector3(room.transform.position.x - 17.5f, 2f, room.transform.position.z), Quaternion.identity,
                            room.transform);
                    }
                }
            }
        }
    }
    
    public void SetRoomCompleted(Vector3 roomPos, bool roomCompleted)
    {
        int x = (int) roomPos.x / 35;
        int z = (int) roomPos.z / 35;
        
        level[z,x].completed = roomCompleted;
    }
}
