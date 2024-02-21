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

    public GameObject[] editableRoomTemplates;
    
    private static GameObject[] roomTemplates;

    private Dictionary<int, (GameObject, (float, float, float))> rooms;

    private GameObject[] level;
    
    private Vector3 startPos;

    private void Start()
    {
        // Crimes against God
        roomTemplates = editableRoomTemplates;
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
        level = new GameObject[levelSize * levelSize];
        level = CreateMap();
        GenerateMap(level);
    }

    private GameObject[] CreateMap()
    {
        GameObject[] _level = new GameObject[levelSize * levelSize];

        // Initial Pass, Fill Array
        for (int i = 0; i < levelSize * levelSize; i++)
        {
            double rand = Random.Range(0f, 1.0f);
            
            if (rand < enemyFrequency)
            {
                GameObject room = new GameObject("Room" + i, typeof(Room));
                
                room.GetComponent<Room>().SetID(Random.Range(0,14));
                room.GetComponent<Room>().SetRoomType(Room.RoomType.Enemy);
                _level[i] = room;
                Destroy(room);
            }
            else
            {
                GameObject room = new GameObject("Room" + i, typeof(Room));
                
                room.GetComponent<Room>().SetID(Random.Range(0,14));
                room.GetComponent<Room>().SetRoomType(Room.RoomType.Empty);
                _level[i] = room;
                Destroy(room);
            }
        }

        int randPos = Random.Range(0, levelSize);
        _level[0 + randPos].GetComponent<Room>().SetRoomType(Room.RoomType.Start);
        _level[(levelSize - 1) * levelSize + randPos].GetComponent<Room>().SetRoomType(Room.RoomType.End);
        
        // Process Edges
        for (int i = 0; i < levelSize; i++)
        {
            float rand = Random.Range(0f, 1.0f);
            rand -= (rand % 0.1f);

            if (rand > 0.4f)
            {
                _level[i * levelSize + 0].GetComponent<Room>().SetID(5);
            }
            else
            {
                int[] ids = new[] { 1, 3 };
                int randBool = (Random.value > 0.5f) ? 1 : 0;
                _level[i * levelSize + 0].GetComponent<Room>().SetID(ids[randBool]);
                Debug.Log(_level[i].GetComponent<Room>().GetID());
            }

            if (randPos > 0.4f)
            {
                _level[i * levelSize + (levelSize - 1)].GetComponent<Room>().SetID(6);
            }
            else
            {
                int[] ids = new[] { 2, 4 };
                int randBool = (Random.value > 0.5f) ? 1 : 0;
                _level[i * levelSize + (levelSize - 1)].GetComponent<Room>().SetID(ids[randBool]);
                Debug.Log(_level[i].GetComponent<Room>().GetID());
            }

            if (randPos > 0.4f)
            {
                _level[i].GetComponent<Room>().SetID(7);
            }
            else
            {
                int[] ids = new[] { 1, 2 };
                int randBool = (Random.value > 0.5f) ? 1 : 0;
                _level[i].GetComponent<Room>().SetID(ids[randBool]);
                Debug.Log(_level[i].GetComponent<Room>().GetID());
            }

            if (randPos > 0.4f)
            {
                _level[(levelSize - 1) * levelSize + i].GetComponent<Room>().SetID(8);
            }
            else
            {
                int[] ids = new[] { 3, 4 };
                int randBool = (Random.value > 0.5f) ? 1 : 0;
                _level[(levelSize - 1) * levelSize + i].GetComponent<Room>().SetID(ids[randBool]);
                Debug.Log(_level[i].GetComponent<Room>().GetID());
            }
        }

        // Process Corners
        _level[0].GetComponent<Room>().SetID(1);
        _level[0 + levelSize - 1].GetComponent<Room>().SetID(2);
        _level[(levelSize - 1) * levelSize].GetComponent<Room>().SetID(3);
        _level[(levelSize - 1) * levelSize + levelSize - 1].GetComponent<Room>().SetID(4);
        
        return _level;
    }

    public void GenerateMap(GameObject[] _level)
    {
        for (int i = 0; i < levelSize; i++)
        {
            for (int j = 0; j < levelSize; j++)
            {
                int id = _level[(i * levelSize + j)].GetComponent<Room>().GetID();
                Room.RoomType type = _level[(i * levelSize + j)].GetComponent<Room>().GetRoomType();
                bool completed = _level[(i * levelSize + j)].GetComponent<Room>().GetCompleted();

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

    public GameObject[] GetLevel()
    {
        return level;
    }
    
    public GameObject GetRoom(Vector3 roomPos)
    {
        int x = (int) roomPos.x / levelSize;
        int z = (int) roomPos.z / levelSize;

        return level[x * levelSize + z];
    }

    public Vector3 GetStartPos()
    {
        return startPos;
    }
}
