using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameData
{
    public Vector3 playerPos;
    // public List<Element> elementInventory;
    // public GameObject[] level;
    // public List<(int, bool, Room.RoomType)> roomInfo;
    public int levelSize;
    public int[] ids;
    public bool[] roomsCompleted;
    public Room.RoomType[] roomTypes;
}

public class DataManager : MonoBehaviour
{
    private string gameDataPath;
    private string inGameDataPath;
    
    public static DataManager instance { private set; get; }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameDataPath = Application.persistentDataPath + "/SaveData.json";
        inGameDataPath = Application.persistentDataPath + "/LevelData.json";
        Debug.Log(inGameDataPath);
    }

    public void SaveGameData(GameData gameData)
    {
        string dataToSave = JsonUtility.ToJson(gameData);
        
        File.WriteAllText(gameDataPath, dataToSave);
    }

    public GameData LoadGameData()
    {
        if (File.Exists(gameDataPath))
        {
            string dataToLoad = File.ReadAllText(gameDataPath);
            GameData gameData = JsonUtility.FromJson<GameData>(dataToLoad);

            return gameData;
        }

        return null;
    }

    public void SaveLevelData(GameData gameData)
    {
        string dataToSave = JsonUtility.ToJson(gameData);
        
        Debug.Log(dataToSave);
        File.WriteAllText(inGameDataPath, dataToSave);
    }

    public GameData LoadLevelData()
    {
        if (File.Exists(inGameDataPath))
        {
            string dataToLoad = File.ReadAllText(inGameDataPath);
            GameData gameData = JsonUtility.FromJson<GameData>(dataToLoad);
            
            return gameData;
        }

        return null;
    }
}
