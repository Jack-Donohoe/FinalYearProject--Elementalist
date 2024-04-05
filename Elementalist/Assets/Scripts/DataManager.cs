using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class GameData
{
    public string levelName;
    public Vector3 playerPos;
    public Quaternion playerRotation;
    public int playerMaxHealth;
    public int playerHealth;
    public int playerMaxMagic;
    public int playerMagic;
    public int playerAttack;
    public int playerDefence;
    public List<Element> elements;
    public RoomV2[,] level;
}

public class LevelData
{
    public Vector3 playerPos;
    public Quaternion playerRotation;
    public int playerMaxHealth;
    public int playerHealth;
    public int playerMaxMagic;
    public int playerMagic;
    public int playerAttack;
    public int playerDefence;
    public RoomV2[,] level;
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
        string dataToSave = JsonConvert.SerializeObject(gameData);
        
        Debug.Log(dataToSave);
        File.WriteAllText(gameDataPath, dataToSave);
    }

    public GameData LoadGameData()
    {
        if (File.Exists(gameDataPath))
        {
            string dataToLoad = File.ReadAllText(gameDataPath);
            GameData gameData = JsonConvert.DeserializeObject<GameData>(dataToLoad);
            
            Debug.Log(gameData.level[0,0]);
            return gameData;
        }

        return null;
    }
    
    public void SaveLevelData(LevelData levelData)
    {
        string dataToSave = JsonConvert.SerializeObject(levelData);
        
        Debug.Log(dataToSave);
        File.WriteAllText(inGameDataPath, dataToSave);
    }
    
    public LevelData LoadLevelData()
    {
        if (File.Exists(inGameDataPath))
        {
            string dataToLoad = File.ReadAllText(inGameDataPath);
            LevelData levelData = JsonConvert.DeserializeObject<LevelData>(dataToLoad);
            
            return levelData;
        }
    
        return null;
    }
}
