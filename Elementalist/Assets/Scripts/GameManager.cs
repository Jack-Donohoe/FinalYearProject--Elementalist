using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public string levelName;
    public int levelInt;

    public int playerLevel = 1;
    public int xpToLevelUp = 100;
    public int playerXP = 0;

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
    public List<Element> elementInventory;
    public Element selectedElement;

    // Level Objects
    public Exploration_HUD hud;
    public int SensitivityValue = 1;
    private GameObject map;

    public bool tutorial;

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
        elementInventory = new List<Element> { ElementManager.Instance.FindElement("Water"), ElementManager.Instance.FindElement("Air") };
        selectedElement = elementInventory[0];

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        var loadScene = SceneManager.LoadSceneAsync("Level1");
        levelName = "Level1";
        levelInt = 1;
        tutorial = true;
        ResetPlayerInfo();

        loadScene.completed += (x) =>
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            SensitivityValue = Mathf.RoundToInt(player.GetComponent<PlayerController>().FreeLookCamera.m_XAxis.m_MaxSpeed / 50);
            StartLevel();
        };
    }

    public void LoadNextLevel()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        SensitivityValue = Mathf.RoundToInt(player.GetComponent<PlayerController>().FreeLookCamera.m_XAxis.m_MaxSpeed / 50);
        
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            var loadScene = SceneManager.LoadSceneAsync("Level2");
            loadScene.completed += (x) =>
            {
                levelName = "Level2";
                levelInt = 2;
                StartLevel();
            };
        }
        else if (SceneManager.GetActiveScene().name == "Level2")
        {
            var loadScene = SceneManager.LoadSceneAsync("Level3");
            loadScene.completed += (x) =>
            {
                levelName = "Level3";
                levelInt = 3;
                StartLevel();
            };
        } else if (SceneManager.GetActiveScene().name == "Level3")
        {
            SceneManager.LoadScene("WinScreen");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void SaveGame()
    { 
        Debug.Log("Saving Game");
        map = GameObject.FindGameObjectWithTag("Map");

        List<string> elementsToSave = new List<string>();
        foreach (Element element in elementInventory)
        {
            elementsToSave.Add(element.GetName());
        }
        
        GameData gameData = new GameData
        {
            levelName = levelName,
            levelInt = levelInt,
            playerPos = GameObject.FindGameObjectWithTag("Player").transform.position,
            playerRotation = GameObject.FindGameObjectWithTag("Player").transform.rotation,
            playerLevel = playerLevel,
            playerXp = playerXP,
            playerMaxHealth = max_health,
            playerHealth = health_points,
            playerMaxMagic = max_magic,
            playerMagic = magic_points,
            playerAttack = attack_power,
            playerDefence = defence_power,
            elements = elementsToSave,
            level = map.GetComponent<ProcGenV4>().level
        };
        DataManager.instance.SaveGameData(gameData);
    }

    public void StartLoadGame()
    {
        GameData gameData = DataManager.instance.LoadGameData();

        SceneManager.LoadScene(gameData.levelName);
        levelName = gameData.levelName;
        levelInt = gameData.levelInt;

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

        playerLevel = gameData.playerLevel;
        xpToLevelUp = playerLevel * 100;
        playerXP = gameData.playerXp;
        max_health = gameData.playerMaxHealth;
        health_points = gameData.playerHealth;
        max_magic = gameData.playerMaxMagic;
        magic_points = gameData.playerMagic;
        attack_power = gameData.playerAttack;
        defence_power = gameData.playerDefence;

        elementInventory = new List<Element>();

        foreach (string element in gameData.elements)
        {
            elementInventory.Add(ElementManager.Instance.FindElement(element));
        }

        if (!elementInventory.Contains(selectedElement))
        {
            selectedElement = elementInventory[0];
        }
        
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<Exploration_HUD>();
        StartCoroutine(hud.RemoveLoadingScreen(tutorial));
    }

    private void StartLevel()
    {
        map = GameObject.FindGameObjectWithTag("Map");
        map.GetComponent<ProcGenV4>().OnLevelLoad();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = map.GetComponent<ProcGenV4>().startPos;
        player.GetComponent<CharacterController>().enabled = true;
        
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<Exploration_HUD>();
        hud.pauseMenu.GetComponent<PauseMenu>().SensitivitySlider.value = SensitivityValue;
        
        StartCoroutine(hud.RemoveLoadingScreen(tutorial));
    }
    
    public void StartCombat(Element enemyElement, bool eliteEnemy)
    {
        map = GameObject.FindGameObjectWithTag("Map");
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        SensitivityValue = Mathf.RoundToInt(player.GetComponent<PlayerController>().FreeLookCamera.m_XAxis.m_MaxSpeed / 50);
            
        LevelData levelData = new LevelData
        {
            playerPos = player.transform.position,
            playerRotation = player.transform.rotation,
            level = map.GetComponent<ProcGenV4>().level
        };
        DataManager.instance.SaveLevelData(levelData);

        var loadScene = SceneManager.LoadSceneAsync("CombatScene");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        hud.loadingScreen.gameObject.SetActive(true);
        loadScene.completed += (x) =>
        {
            // Prevents a bug where game freezes if player character collides with enemy right as menu is opened
            Time.timeScale = 1f;
            
            GameObject manager = GameObject.FindGameObjectWithTag("CombatManager");
            StartCoroutine(manager.GetComponent<Combat_Manager>().StartUp(enemyElement, eliteEnemy));
        };
    }

    public void ReturnToLevel()
    {
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
        
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<Exploration_HUD>();
        hud.pauseMenu.GetComponent<PauseMenu>().SensitivitySlider.value = SensitivityValue;
        hud.dialogueMenu.GetComponent<DialogueMenu>().inventoryTutorialMenu.SetActive(false);

        tutorial = false;
        
        StartCoroutine(hud.RemoveLoadingScreen(tutorial));
    }

    public bool CheckLevelUp(int xpToAdd)
    {
        playerXP += xpToAdd;

        if (playerXP >= xpToLevelUp)
        {
            playerXP -= xpToLevelUp;
            
            int NewMaxHP = max_health + ((Random.value > 0.3f) ? 20 : 25);
            int NewMaxMP = max_magic + ((Random.value > 0.5f) ? 10 : 15);
            int NewAttack = attack_power + Random.Range(5, 10);
            int NewDefence = defence_power + Random.Range(5, 10);

            CombatHUD hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<CombatHUD>();
            hud.LevelUpHP.text = "Max HP: " + max_health + " + " + (NewMaxHP - max_health) + " = " + NewMaxHP; 
            hud.LevelUpMP.text = "Max MP: " + max_magic + " + " + (NewMaxMP - max_magic) + " = " + NewMaxMP; 
            hud.LevelUpAttack.text = "Attack Power: " + attack_power + " + " + (NewAttack - attack_power) + " = " + NewAttack; 
            hud.LevelUpDefence.text = "Defence Power: " + defence_power + " + " + (NewDefence - defence_power) + " = " + NewDefence; 
            
            SetPlayerStats(NewMaxHP, NewMaxMP, NewAttack, NewDefence);
            SetPlayerResources(NewMaxHP, NewMaxMP);
            
            playerLevel++;
            xpToLevelUp = playerLevel * 100;

            return true;
        }

        return false;
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

    public void SetPlayerResources(int hp, int mp)
    {
        health_points = hp;
        magic_points = mp;
        Debug.Log("HP=" + hp +" MP=" + mp);
    }
    
    private void SetPlayerStats(int maxHP, int maxMP, int attackPow, int defencePow)
    {
        max_health = maxHP;
        max_magic = maxMP;
        attack_power = attackPow;
        defence_power = defencePow;
    }

    private void ResetPlayerInfo()
    {
        playerLevel = 1;
        xpToLevelUp = playerLevel * 100;
        playerXP = 0;
        max_health = 100;
        max_magic = 60;
        health_points = 100;
        magic_points = 60;
        attack_power = 10;
        defence_power = 5;
    }
}
