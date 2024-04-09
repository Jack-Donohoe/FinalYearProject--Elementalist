using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public string levelName;

    // Player Info
    private int max_health = 100;
    public int Max_Health
    {
        get => max_health;
        set => max_health = value;
    }
    
    private int health_points = 100;
    public int HP
    {
        get => health_points;
        set => health_points = value;
    }
    
    private int max_magic = 60;
    public int Max_Magic
    {
        get => max_magic;
        set => max_magic = value;
    }
    
    private int magic_points = 60;
    public int MP
    {
        get => magic_points;
        set => magic_points = value;
    }
    
    private int attack_power = 1;
    public int Attack_Power
    {
        get => attack_power;
        set => attack_power = value;
    }
    
    private int defence_power = 5;
    public int Defence_Power
    {
        get => defence_power;
        set => defence_power = value;
    }
    
    // Element Info
    public List<Element> elementInventory = new List<Element>();
    public Element selectedElement;

    // Level Objects
    public Exploration_HUD hud;
    private GameObject map;
    public GameObject Map => map;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
    }

    private void Start()
    {
        selectedElement = elementInventory[0];
    }

    public void LoadMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("StartScreen");
    }

    public void StartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("Level1");
        levelName = "Level1";
        ResetPlayerInfo();
        StartCoroutine(StartLevel());
    }

    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            SceneManager.LoadScene("Level2");
            StartCoroutine(StartLevel());
            levelName = "Level2";
        }
        else if (SceneManager.GetActiveScene().name == "Level2")
        {
            SceneManager.LoadScene("Level3");
            StartCoroutine(StartLevel());
            levelName = "Level3";
        } else if (SceneManager.GetActiveScene().name == "Level3")
        {
            SceneManager.LoadScene("WinScreen");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void SaveGame()
    { 
        map = GameObject.FindGameObjectWithTag("Map");
        
        GameData gameData = new GameData
        {
            levelName = levelName,
            playerPos = GameObject.FindGameObjectWithTag("Player").transform.position,
            playerRotation = GameObject.FindGameObjectWithTag("Player").transform.rotation,
            playerMaxHealth = max_health,
            playerHealth = health_points,
            playerMaxMagic = max_magic,
            playerMagic = magic_points,
            playerAttack = attack_power,
            playerDefence = defence_power,
            //elements = elementInventory,
            level = map.GetComponent<ProcGenV4>().level
        };
        DataManager.instance.SaveGameData(gameData);
    }

    public void StartLoadGame()
    {
        GameData gameData = DataManager.instance.LoadGameData();

        SceneManager.LoadScene(gameData.levelName);
        levelName = gameData.levelName;

        StartCoroutine(LoadGame(gameData));
    }

    private IEnumerator LoadGame(GameData gameData)
    {
        yield return new WaitForNextFrameUnit();
        
        map = GameObject.FindGameObjectWithTag("Map");
        
        map.GetComponent<ProcGenV4>().GenerateLevel(gameData.level);
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = gameData.playerPos;
        player.GetComponent<CharacterController>().enabled = true;
        player.transform.rotation = gameData.playerRotation;
        
        StartCoroutine(RemoveLoadingScreen());
    }

    private IEnumerator StartLevel()
    {
        yield return new WaitForNextFrameUnit();
        map = GameObject.FindGameObjectWithTag("Map");
        map.GetComponent<ProcGenV4>().OnLevelLoad();
        
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").transform.position = map.GetComponent<ProcGenV4>().startPos;
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().enabled = true;
        
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<Exploration_HUD>();
        StartCoroutine(RemoveLoadingScreen());
    }

    private IEnumerator RemoveLoadingScreen()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<Exploration_HUD>();
        hud.loadingScreen.gameObject.SetActive(false);
        hud.inGameHUD.gameObject.SetActive(true);
    }
    
    public void StartCombat(Element enemyElement)
    {
        map = GameObject.FindGameObjectWithTag("Map");
            
        LevelData levelData = new LevelData
        {
            playerPos = GameObject.FindGameObjectWithTag("Player").transform.position,
            playerRotation = GameObject.FindGameObjectWithTag("Player").transform.rotation,
            playerMaxHealth = max_health,
            playerHealth = health_points,
            playerMaxMagic = max_magic,
            playerMagic = magic_points,
            playerAttack = attack_power,
            playerDefence = defence_power,
            level = map.GetComponent<ProcGenV4>().level
        };
        DataManager.instance.SaveLevelData(levelData);

        var loadScene = SceneManager.LoadSceneAsync("CombatScene");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        hud.loadingScreen.gameObject.SetActive(true);
        loadScene.completed += (x) =>
        {
            GameObject manager = GameObject.FindGameObjectWithTag("CombatManager");
            StartCoroutine(manager.GetComponent<Combat_Manager>().StartUp(enemyElement));
        };
    }

    public IEnumerator ReturnToLevel()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(levelName);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(ReloadLevel());
    }

    private IEnumerator ReloadLevel()
    {
        yield return new WaitForNextFrameUnit();
        LevelData levelData = DataManager.instance.LoadLevelData();
        map = GameObject.FindGameObjectWithTag("Map");
        
        map.GetComponent<ProcGenV4>().GenerateLevel(levelData.level);
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = levelData.playerPos;
        player.GetComponent<CharacterController>().enabled = true;
        player.transform.rotation = levelData.playerRotation;
        
        StartCoroutine(RemoveLoadingScreen());
    }

    public IEnumerator LoseGame()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("DeathScreen");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public string AddElement(Element element)
    {
        if (!elementInventory.Contains(element))
        {
            elementInventory.Add(element);
            return "You Created: " + element.GetName();
        }

        return "Element Already Exists";
    }

    public string RemoveElement(Element element)
    {
        if (elementInventory.Contains(element))
        {
            elementInventory.Remove(element);
            return "Removed: " + element.GetName();
        }

        return "Element Does Not Exist";
    }
    
    public Element GetElement(int i)
    {
        return elementInventory[i];
    }

    public List<Element> GetElements()
    {
        return elementInventory;
    }

    public void SetPlayerResources(int hp, int mp)
    {
        health_points = hp;
        magic_points = mp;
        Debug.Log("HP=" + hp +" MP=" + mp);
    }
    
    public void SetPlayerStats(int maxHP, int maxMP, int attackPow, int defencePow)
    {
        max_health = maxHP;
        max_magic = maxMP;
        attack_power = attackPow;
        defence_power = defencePow;
    }

    private void ResetPlayerInfo()
    {
        max_health = 100;
        max_magic = 60;
        health_points = 100;
        magic_points = 60;
        attack_power = 10;
        defence_power = 5;
    }
}
