using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcGenV4 : MonoBehaviour
{
    private RoomV2[,] level;
    public int levelSize;
    
    public GameObject[] roomTemplates;

    public Dictionary<(bool, bool, bool, bool), (GameObject, float)> rooms;
    
    public Vector3 startPos { get; set; }
        
    // Start is called before the first frame update
    void Start()
    {
        rooms = new Dictionary<(bool, bool, bool, bool), (GameObject, float)>()
        {
            // One Entrance Rooms
            { (true, false, false, false), (roomTemplates[0], -90f) },
            { (false, true, false, false), (roomTemplates[0], 180f) },
            { (false, false, true, false), (roomTemplates[0], 90f) },
            { (false, false, false, true), (roomTemplates[0], 0f) },
            // Corner Rooms
            { (true, true, false, false), (roomTemplates[1], 180f) },
            { (true, false, false, true), (roomTemplates[1], -90f) },
            { (false, true, true, false), (roomTemplates[1], 90f) },
            { (false, false, true, true), (roomTemplates[1], 0f) },
            // Straight Rooms
            { (true, false, true, false), (roomTemplates[2], 0f) },
            { (false, true, false, true), (roomTemplates[2], 90f) },
            // T Junction Rooms
            { (true, true, false, true), (roomTemplates[3], 180f) },
            { (true, true, true, false), (roomTemplates[3], 90f) },
            { (false, true, true, true), (roomTemplates[3], 0f) },
            { (true, false, true, true), (roomTemplates[3], -90f) },
            // Four Entrance Room
            { (true, true, true, true), (roomTemplates[4], 0f) }
        };
        
        level = PopulateMap();
        GenerateLevel(level);
    }

    RoomV2[,] PopulateMap()
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

                room.connections["top"] = Random.Range(0, 10) <= 5;
                room.connections["left"] = Random.Range(0, 10) <= 5;
                room.connections["bottom"] = Random.Range(0, 10) <= 5;
                room.connections["right"] = Random.Range(0, 10) <= 5;

                currentLevel[i, j] = room;
            }
        }
        
        // Process corners
        currentLevel[0, 0].SetAllConnections((false,false,true,true));
        currentLevel[0, levelSize - 1].SetAllConnections((false,true,true,false));
        currentLevel[levelSize - 1, 0].SetAllConnections((true,false,false,true));
        currentLevel[levelSize - 1, levelSize - 1].SetAllConnections((true,true,false,false));

        // Process Edges
        for (int i = 1; i < levelSize - 1; i++)
        {
            currentLevel[0, i].connections["top"] = false;
            currentLevel[levelSize - 1, i].connections["bottom"] = false;
            currentLevel[i, 0].connections["left"] = false;
            currentLevel[i, levelSize - 1].connections["right"] = false;
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
                    currentLevel[i, j].connections["false"] = true;
                }
            }
        }

        return currentLevel;
    }

    void GenerateLevel(RoomV2[,] levelToGenerate)
    {
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
                        room.name = "Room" + (i * levelSize + j);
                        room.transform.Rotate(0f, roomInfo.Item2, 0f);

                        if (levelToGenerate[i, j].roomType == RoomV2.RoomType.Enemy)
                        {
                            if (levelToGenerate[i, j].completed == false)
                            {
                                //room.GetComponent<Level_Room>().SpawnEnemy();
                            }
                        }
                    }
                }
                else
                {
                    startPos = new Vector3(j * 35f, 0f, i * 35f);
                }
            }
        }
    }
}
