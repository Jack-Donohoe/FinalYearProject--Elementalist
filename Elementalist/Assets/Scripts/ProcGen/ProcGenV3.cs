using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProcGenV3 : MonoBehaviour
{
    public int levelSize;
    
    [Range(0,1)]
    public double enemyFrequency;
    
    public GameObject[] roomTemplates;

    private Dictionary<int, (GameObject, (float, float, float))> rooms;

    //private List<GameObject> level;

    private int[] ids;

    private bool[] roomsCompleted;

    private Room.RoomType[] roomTypes;
    
    private Vector3 startPos;

    private void Start()
    {
        rooms = new Dictionary<int, (GameObject, (float, float, float))>()
        {
            {0,(roomTemplates[0], (0f, 0f, 0f))},
            {1,(roomTemplates[1], (0f, 90f, 0f))},
            {2,(roomTemplates[1], (0f, 0f, 0f))},
            {3,(roomTemplates[1], (0f, 180f, 0f))},
            {4,(roomTemplates[1], (0f, -90f, 0f))},
            {5,(roomTemplates[2], (0f, 90f, 0f))},
            {6,(roomTemplates[2], (0f, -90f, 0f))},
            {7,(roomTemplates[2], (0f, 0f, 0f))},
            {8,(roomTemplates[2], (0f, 180f, 0f))},
            {9,(roomTemplates[3], (0f, 0f, 0f))},
            {10,(roomTemplates[3], (0f, 90f, 0f))},
            {11,(roomTemplates[4], (0f, 0f, 0f))},
            {12,(roomTemplates[4], (0f, 90f, 0f))},
            {13,(roomTemplates[4], (0f, 180f, 0f))},
            {14,(roomTemplates[4], (0f, -90f, 0f))},
        };
    }

    public void OnLevelLoad()
    {
        CreateMap();
        GenerateMap(ids, roomsCompleted, roomTypes);
    }

    private void CreateMap()
    {
        ids = new int[levelSize * levelSize];
        roomsCompleted = new bool[levelSize * levelSize];
        roomTypes = new Room.RoomType[levelSize * levelSize];
        
        // Initial Pass, Fill Array
        for (int i = 0; i < levelSize * levelSize; i++)
        {
            double rand = Random.Range(0f, 1.0f);
            
            if (rand < enemyFrequency)
            {
                ids[i] = Random.Range(0,14);
                roomsCompleted[i] = false;
                roomTypes[i] = Room.RoomType.Enemy;
            }
            else
            {
                ids[i] = Random.Range(0,14);
                roomsCompleted[i] = false;
                roomTypes[i] = Room.RoomType.Empty;
            }
        }

        int randPos = Random.Range(0, levelSize);
        roomTypes[0 + randPos] = Room.RoomType.Start;
        roomTypes[(levelSize - 1) * levelSize + randPos] = Room.RoomType.End;
        
        // Process Edges
        for (int i = 0; i < levelSize; i++)
        {
            float rand = Random.Range(0f, 1.0f);
            rand -= (rand % 0.1f);
            Debug.Log(rand);

            if (rand >= 0.8f)
            {
                ids[i * levelSize + 0] = 5;
            }
            else
            {
                int[] idsToSelect = new[] { 1, 3, 9 };
                int randBool = (Random.value > 0.5f) ? 1 : 0;
                ids[i * levelSize + 0] =  idsToSelect[randBool];
            }

            if (randPos >= 0.8f)
            {
                ids[i * levelSize + (levelSize - 1)] = 6;
            }
            else
            {
                int[] idsToSelect = new[] { 2, 4, 9 };
                int randBool = (Random.value > 0.5f) ? 1 : 0;
                ids[i * levelSize + (levelSize - 1)] = idsToSelect[randBool];
            }

            if (randPos >= 0.8f)
            {
                ids[i] = 7;
            }
            else
            {
                int[] idsToSelect = new[] { 1, 2, 10 };
                int randBool = (Random.value > 0.5f) ? 1 : 0;
                ids[i] =  idsToSelect[randBool];
            }

            if (randPos >= 0.8f)
            {
                ids[(levelSize - 1) * levelSize + i] = 8;
            }
            else
            {
                int[] idsToSelect = new[] { 3, 4, 10 };
                int randBool = (Random.value > 0.5f) ? 1 : 0;
                ids[(levelSize - 1) * levelSize + i] =  idsToSelect[randBool];
            }
        }

        // Process Corners
        ids[0] = 1;
        ids[0 + levelSize - 1] = 2;
        ids[(levelSize - 1) * levelSize] = 3;
        ids[(levelSize - 1) * levelSize + levelSize - 1] = 4;
    }

    public void GenerateMap(int[] ids, bool[] roomsCompleted, Room.RoomType[] roomTypes)
    {
        for (int i = 0; i < levelSize; i++)
        {
            for (int j = 0; j < levelSize; j++)
            {
                int id = ids[(i * levelSize + j)];
                bool completed = roomsCompleted[(i * levelSize + j)];
                Room.RoomType type = roomTypes[(i * levelSize + j)];

                if (rooms.TryGetValue(id, out (GameObject, (float, float, float)) roomInfo))
                {
                    GameObject room = Instantiate(roomInfo.Item1, new Vector3(j * 20f, 0f, i * 20f), Quaternion.identity, this.transform);
                    room.name = "Room" + (i * levelSize + j);
                    room.transform.Rotate(roomInfo.Item2.Item1, roomInfo.Item2.Item2, roomInfo.Item2.Item3);
                    
                    if (type == Room.RoomType.Start)
                    {
                        startPos = new Vector3(j * 20f, 0f, i * 20f);
                    } else if (type == Room.RoomType.Enemy)
                    {
                        if (completed == false)
                        {
                            room.GetComponent<Level_Room>().SpawnEnemy();
                        }
                    }
                }
            }
        }
    }

    public int[] GetIds()
    {
        return ids;
    }

    public void SetIds(int[] ids)
    {
        this.ids = ids;
    }

    public bool[] GetRoomsCompleted()
    {
        return roomsCompleted;
    }

    public void SetRoomsCompleted(bool[] roomsCompleted)
    {
        this.roomsCompleted = roomsCompleted;
    }

    public Room.RoomType[] GetRoomTypes()
    {
        return roomTypes;
    }

    public void SetRoomTypes(Room.RoomType[] roomTypes)
    {
        this.roomTypes = roomTypes;
    }
    
    public (int,bool,Room.RoomType) GetRoom(Vector3 roomPos)
    {
        int x = (int) roomPos.x / 20;
        int z = (int) roomPos.z / 20;

        return (ids[x * levelSize + z], roomsCompleted[x * levelSize + z], roomTypes[x * levelSize + z]);
    }

    public void SetRoomCompleted(Vector3 roomPos, bool roomCompleted)
    {
        int x = (int) roomPos.x / 20;
        int z = (int) roomPos.z / 20;
        Debug.Log(z * levelSize + x);

        roomsCompleted[z * levelSize + x] = roomCompleted;
    }

    public Vector3 GetStartPos()
    {
        return startPos;
    }
}
