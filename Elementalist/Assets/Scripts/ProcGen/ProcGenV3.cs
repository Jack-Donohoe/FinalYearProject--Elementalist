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
            {1,(roomTemplates[1], (0f, 0f, 0f))},
            {2,(roomTemplates[1], (0f, 0f, 0f))},
            {3,(roomTemplates[1], (0f, 0f, 0f))},
            {4,(roomTemplates[1], (0f, 0f, 0f))},
            {5,(roomTemplates[2], (0f, 0f, 0f))},
            {6,(roomTemplates[2], (0f, 0f, 0f))},
            {7,(roomTemplates[2], (0f, 0f, 0f))},
            {8,(roomTemplates[2], (0f, 0f, 0f))},
            {9,(roomTemplates[3], (0f, 0f, 0f))},
            {10,(roomTemplates[3], (0f, 0f, 0f))},
            {11,(roomTemplates[4], (0f, 0f, 0f))},
            {12,(roomTemplates[4], (0f, 0f, 0f))},
            {13,(roomTemplates[4], (0f, 0f, 0f))},
            {14,(roomTemplates[4], (0f, 0f, 0f))},
        };
        
        level = new GameObject[levelSize * levelSize];
        level = CreateMap();
        GenerateMap(level);
    }

    private GameObject[] CreateMap()
    {
        GameObject[] _level = new GameObject[levelSize * levelSize];

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
        
        for (int i = 0; i < levelSize; i++)
        {
            _level[i * levelSize + 0].GetComponent<Room>().SetID(5);
            _level[i * levelSize + (levelSize - 1)].GetComponent<Room>().SetID(6);
            _level[0 * levelSize + i].GetComponent<Room>().SetID(7);
            _level[(levelSize - 1) * levelSize + i].GetComponent<Room>().SetID(8);
        }
        
        return _level;
    }

    public void GenerateMap(GameObject[] _level)
    {
        for (int i = 0; i < levelSize; i++)
        {
            for (int j = 0; j < levelSize; j++)
            {
                int id = _level[i*j].GetComponent<Room>().GetID();
                Debug.Log(id);

                if (rooms.TryGetValue(id, out (GameObject, (float, float, float)) roomInfo))
                {
                    GameObject room = Instantiate(roomInfo.Item1, new Vector3(i * 20f, 0f, j * 20f), Quaternion.identity, this.transform);
                    room.transform.Rotate(roomInfo.Item2.Item1, roomInfo.Item2.Item2, roomInfo.Item2.Item3);
                }
            }
        }
    }

    public GameObject GetRoom(Vector3 roomPos)
    {
        int x = (int) roomPos.x / levelSize;
        int z = (int) roomPos.z / levelSize;

        return level[z * levelSize + x];
    }
}
