using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { private set; get; }

    private int score;

    public List<Element> _elements = new List<Element>();

    public Exploration_HUD hud;

    private GameObject map;

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

    public void LoadLevel()
    {
        score = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("Level1");
        StartCoroutine(StartLevel());
    }

    IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(0.01f);
        map = GameObject.FindGameObjectWithTag("Map");
        map.GetComponent<ProcGenV2>().OnLevelLoad();
    }
    
    public void StartCombat()
    {
        map = GameObject.FindGameObjectWithTag("Map");
            
        GameData levelData = new GameData
        {
            playerPos = GameObject.FindGameObjectWithTag("Player").transform.position,
            elementInventory = _elements,
            level = map.GetComponent<ProcGenV2>().GetLevel()
        };

        DataManager.instance.SaveLevelData(levelData);

        SceneManager.LoadScene("CombatScene");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReturnToLevel()
    {
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

    IEnumerator ReloadLevel()
    {
        yield return new WaitForSeconds(0.1f);
        GameData levelData = DataManager.instance.LoadLevelData();
        Debug.Log(levelData.playerPos);
        GameObject.FindGameObjectWithTag("Player").transform.position = levelData.playerPos;
            
        map = GameObject.FindGameObjectWithTag("Map");
        int levelSize = map.GetComponent<ProcGenV2>().levelSize;
        int[,] level = new int[levelSize, levelSize];
            
        for (int x = 0; x < levelSize; x++)
        {
            for (int z = 0; z < levelSize; z++)
            {
                level[x, z] = levelData.level[x * levelSize + z];
            }
        }
            
        map.GetComponent<ProcGenV2>().GenerateMap(level);

        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<Exploration_HUD>();
        hud.SetScoreText("Enemies Defeated: " + score);
    }

    public void LoseGame()
    {
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
}
