using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcGenV2 : MonoBehaviour
{
    public int levelSize;
    public int roomAmount;
    
    public GameObject[] emptyRooms;
    public GameObject[] enemyRooms;

    public double enemyFrequency;

    private int[,] level;
    
    // Start is called before the first frame update
    void Start()
    {
        level = new int[levelSize,levelSize];
        
        GenerateMap();
    }

    List<(int,int)> PlaceRooms()
    {
        List<(int, int)> roomLocations = new List<(int, int)>();
        
        int rand = Random.Range(0, levelSize - 1);

        level[levelSize - 1, rand] = 1; 
        
        rand = Random.Range(0, levelSize - 1);

        level[0, rand] = 2;
        
        for (int i = 0; i < roomAmount; i++)
        {
            int x = Random.Range(0, levelSize - 1);
            int z = Random.Range(0, levelSize - 1);

            if (level[x, z] == 0)
            {
                level[x, z] = 3;
            }
            else
            {
                x = Random.Range(0, levelSize - 1);
                z = Random.Range(0, levelSize - 1);

                level[x, z] = 3;
            }

            Debug.Log("X: " + x + " Z: " + z);
            roomLocations.Add((x,z));
        }

        return roomLocations;
    }

    void CreateHallways(List<(int, int)> roomLocations)
    {
        List<List<(int, int)>> paths = new List<List<(int, int)>>();
    }

    void GenerateMap()
    {
        List<(int, int)> roomLocations = PlaceRooms();
        
        for (int i = 0; i < levelSize; i++)
        {
            for (int j = 0; j < levelSize; j++)
            {
                if (level[i, j] == 1 || level[i, j] == 2)
                {
                    GameObject room = (Instantiate(emptyRooms[0], new Vector3(i * 20f, 0f, j * 20f), Quaternion.identity, this.transform));
                } else if (level[i, j] == 3)
                {
                    GameObject room = (Instantiate(emptyRooms[0], new Vector3(i * 20f, 0f, j * 20f), Quaternion.identity, this.transform));
                }
            }
        }
        
        CreateHallways(roomLocations);
    }
}
