using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class DemoMap : MonoBehaviour
{
    [SerializeField] GameObject ground;
    [SerializeField] GameObject wall;
    [SerializeField] GameObject corner;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    void GenerateMap() 
    {
        int[] level = {2, 1, 1, 1, 1, 1, 1, 1, 1, 2,
                           1, 0, 0, 1, 0, 0, 0, 0, 0, 1,
                           1, 0, 1, 2, 0, 0, 0, 0, 1, 2,
                           1, 0, 0, 0, 0, 0, 0, 1, 2, 0,
                           2, 1, 1, 1, 0, 0, 1, 2, 0, 1,
                           1, 0, 0, 0, 0, 0, 0, 0, 0, 1,
                           1, 0, 2, 0, 0, 0, 1, 0, 1, 2,
                           1, 1, 2, 0, 0, 0, 1, 0, 0, 1,
                           1, 0, 0, 1, 0, 0, 0, 0, 0, 1,
                           1, 0, 1, 2, 0, 0, 0, 0, 1, 2};

        for (int i = 0; i < level.Length; i++)
        {
            
            if (level[i] == 0)
            {
                int column, row;
                column = i % 10;
                row = i / 10;
                GameObject object1 = (GameObject)(Instantiate(ground, new Vector3(column * 10.0f - 50f, 0f, row * 10.0f - 5f), Quaternion.identity, this.transform));
                print(column + ", " + row);
            }
            else if (level[i] == 1)
            {
                int column, row;
                column = i % 10;
                row = i / 10;
                GameObject object2 = (GameObject)(Instantiate(wall, new Vector3(column * 10.0f - 50f, 0f, row * 10.0f - 5f), Quaternion.identity, this.transform));
                print(column + ", " + row);
            }
            else if (level[i] == 2)
            {
                int column, row;
                column = i % 10;
                row = i / 10;
                GameObject object3 = (GameObject)(Instantiate(corner, new Vector3(column * 10.0f - 50f, 0f, row * 10.0f - 5f), Quaternion.identity, this.transform));
                print(column + ", " + row);
            }
        }
        
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }
}
