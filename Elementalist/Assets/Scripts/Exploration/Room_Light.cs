using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Light : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.levelName == "Level1")
        {
            GetComponent<Light>().color = new Color(0f, 0.95f, 1f);
        }
        else if (GameManager.Instance.levelName == "Level2")
        {
            GetComponent<Light>().color = new Color(0f, 1f, 0f);
        } else if (GameManager.Instance.levelName == "Level3")
        {
            GetComponent<Light>().color = new Color(1f, 0f, 0f);
        }
    }
}
