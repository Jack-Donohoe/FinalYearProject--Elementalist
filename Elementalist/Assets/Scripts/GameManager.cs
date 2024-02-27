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

    private int score;

    // Player Info
    private int health_points = 100;
    private int magic_points = 50;
    private int attack_power = 10;
    private int defence_power = 5;
    
    // Element Info
    public List<Element> _elements = new List<Element>();
    public Element selectedElement;

    // Level Objects
    public Exploration_HUD hud;
    private GameObject map;

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
        selectedElement = _elements[0];
    }

    public void LoadMainMenu()
    {
        score = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("StartScreen");
    }

    public void LoadLevel()
    {
        score = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("Level1");
        StartCoroutine(StartLevel());
    }

    private IEnumerator StartLevel()
    {
        yield return new WaitForNextFrameUnit();
        map = GameObject.FindGameObjectWithTag("Map");
        map.GetComponent<ProcGenV3>().OnLevelLoad();
        
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().enabled = false;
        GameObject.FindGameObjectWithTag("Player").transform.position = map.GetComponent<ProcGenV3>().GetStartPos();
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().enabled = true;
        
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<Exploration_HUD>();
        StartCoroutine(RemoveLoadingScreen());
    }

    private IEnumerator RemoveLoadingScreen()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        hud.loadingScreen.gameObject.SetActive(false);
        hud.inGameHUD.gameObject.SetActive(true);
    }
    
    public void StartCombat()
    {
        map = GameObject.FindGameObjectWithTag("Map");
            
        GameData levelData = new GameData
        {
            playerPos = GameObject.FindGameObjectWithTag("Player").transform.position,
            playerRotation = GameObject.FindGameObjectWithTag("Player").transform.rotation,
            playerHealth = health_points,
            playerMagic = magic_points,
            playerAttack = attack_power,
            playerDefence = defence_power,
            levelSize = map.GetComponent<ProcGenV3>().GetIds().Length,
            ids = map.GetComponent<ProcGenV3>().GetIds(),
            roomsCompleted = map.GetComponent<ProcGenV3>().GetRoomsCompleted(),
            roomTypes = map.GetComponent<ProcGenV3>().GetRoomTypes()
        };
        DataManager.instance.SaveLevelData(levelData);

        SceneManager.LoadScene("CombatScene");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public IEnumerator ReturnToLevel()
    {
        yield return new WaitForSeconds(2f);
        
        if (score == 4)
        {
            SceneManager.LoadScene("WinScreen");
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            SceneManager.LoadScene("Level1");

            StartCoroutine(ReloadLevel());
            
            score++;
        }
    }

    private IEnumerator ReloadLevel()
    {
        yield return new WaitForNextFrameUnit();
        GameData levelData = DataManager.instance.LoadLevelData();
        map = GameObject.FindGameObjectWithTag("Map");
        
        // Reinitialize arrays in map with saved arrays
        map.GetComponent<ProcGenV3>().SetIds(levelData.ids);
        map.GetComponent<ProcGenV3>().SetRoomsCompleted(levelData.roomsCompleted);
        map.GetComponent<ProcGenV3>().SetRoomTypes(levelData.roomTypes);
        
        map.GetComponent<ProcGenV3>().GenerateMap(levelData.ids, levelData.roomsCompleted, levelData.roomTypes);

        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<Exploration_HUD>();
        hud.SetScoreText("Enemies Defeated: " + score);
        
        Debug.Log(levelData.playerRotation);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = levelData.playerPos;
        player.transform.rotation = levelData.playerRotation;
        player.GetComponent<CharacterController>().enabled = true;
        
        StartCoroutine(RemoveLoadingScreen());
    }

    public IEnumerator LoseGame()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("DeathScreen");
    }

    public void AddElement(Element element)
    {
        _elements.Add(element);
    }
    
    public Element GetElement(int i)
    {
        return _elements[i];
    }

    public List<Element> GetElements()
    {
        return _elements;
    }

    public (int, int, int, int) GetPlayerInfo()
    {
        return (health_points, magic_points, attack_power, defence_power);
    }

    public void SetPlayerInfo((int,int,int,int) playerInfo)
    {
        health_points = playerInfo.Item1;
        magic_points = playerInfo.Item2;
        attack_power = playerInfo.Item3;
        defence_power = playerInfo.Item4;
    }
}
