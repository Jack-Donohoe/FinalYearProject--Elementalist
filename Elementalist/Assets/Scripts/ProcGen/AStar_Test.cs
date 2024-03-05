using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar_Test : MonoBehaviour
{
    // List<(int,int)> PlaceRooms()
    // {
    //     List<(int, int)> roomLocations = new List<(int, int)>();
    //     
    //     int rand = Random.Range(0, levelSize - 1);
    //
    //     level[levelSize - 1, rand] = 1; 
    //     roomLocations.Add((levelSize - 1,rand));
    //     
    //     rand = Random.Range(0, levelSize - 1);
    //
    //     level[0, rand] = 2;
    //     roomLocations.Add((0, rand));
    //     
    //     // for (int i = 0; i < roomAmount; i++)
    //     // {
    //     //     int x = Random.Range(0, levelSize - 1);
    //     //     int z = Random.Range(0, levelSize - 1);
    //     //
    //     //     if (level[x, z] == 0)
    //     //     {
    //     //         level[x, z] = 3;
    //     //     }
    //     //     else
    //     //     {
    //     //         x = Random.Range(0, levelSize - 1);
    //     //         z = Random.Range(0, levelSize - 1);
    //     //
    //     //         level[x, z] = 3;
    //     //     }
    //     //
    //     //     Debug.Log("X: " + x + " Z: " + z);
    //     //     roomLocations.Add((x,z));
    //     // }
    //
    //     return roomLocations;
    // }
    //
    // void CreateHallways(List<(int, int)> roomLocations)
    // {
    //     List<List<(int, int)>> paths = new List<List<(int, int)>>();
    //
    //     for (int i = 0; i < roomLocations.Count - 1; i++)
    //     {
    //         Vector2Int startPos = new Vector2Int(roomLocations[i].Item1 * 20, roomLocations[i].Item2 * 20);
    //         Vector2Int goalPos = new Vector2Int(roomLocations[i + 1].Item1 * 20, roomLocations[i + 1].Item2 * 20);
    //         Vector2Int lastPos;
    //
    //     }
    // }
    //
    // void GenerateMap()
    // {
    //     List<(int, int)> roomLocations = PlaceRooms();
    //     
    //     for (int i = 0; i < levelSize; i++)
    //     {
    //         for (int j = 0; j < levelSize; j++)
    //         {
    //             if (level[i, j] == 1 || level[i, j] == 2)
    //             {
    //                 GameObject room = (Instantiate(emptyRooms[0], new Vector3(i * 20f, 0f, j * 20f), Quaternion.identity, this.transform));
    //             } else if (level[i, j] == 3)
    //             {
    //                 GameObject room = (Instantiate(emptyRooms[0], new Vector3(i * 20f, 0f, j * 20f), Quaternion.identity, this.transform));
    //             }
    //             else
    //             {
    //                 Vector3 pos = new Vector3(i * 20f + 10f, 0, j * 20f);
    //                 GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //                 wall.transform.localScale = new Vector3(20f, 1, 20f);
    //                 wall.transform.position = pos;
    //             }
    //         }
    //     }
    //     
    //     CreateHallways(roomLocations);
    // }
}
