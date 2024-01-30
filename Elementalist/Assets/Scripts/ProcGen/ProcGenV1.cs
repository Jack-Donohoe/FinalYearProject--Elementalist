using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class ProcGenV1 : MonoBehaviour
{
    [SerializeField] GameObject room;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    void GenerateMap() 
    {
        int[] level = {1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                           1, 0, 0, 1, 0, 0, 0, 0, 0, 1,
                           1, 1, 1, 1, 0, 0, 0, 0, 1, 1,
                           1, 0, 0, 0, 0, 0, 0, 1, 1, 0,
                           1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                           1, 0, 1, 0, 0, 0, 0, 0, 0, 1,
                           1, 0, 1, 0, 0, 0, 1, 1, 1, 1,
                           1, 1, 1, 0, 0, 0, 1, 0, 0, 1,
                           1, 0, 1, 1, 0, 0, 0, 0, 0, 1,
                           1, 0, 1, 1, 1, 1, 1, 1, 1, 1};

        for (int i = 0; i < level.Length; i++)
        {
            
            if (level[i] == 1)
            {
                int column, row;
                column = i % 10;
                row = i / 10;
                GameObject object1 = (GameObject)(Instantiate(room, new Vector3(column * 20f, 0f, row * 20f), Quaternion.identity, this.transform));
                print(column + ", " + row);
            }
        }
    }
}
