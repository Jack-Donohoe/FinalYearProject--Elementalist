using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Scene level;

    private GameObject level_content;
    
    public static GameManager instance { private set; get; }

    private int score;

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

        level = SceneManager.GetSceneByName("Level1");
    }

    public void Update()
    {
        if (level_content == null)
        {
            level_content = GameObject.Find("Level");
            score = 0;
        }
    }

    public void LoadLevel()
    {
        score = 0;
        SceneManager.LoadScene("Level1");
    }
    
    public void StartCombat()
    {
        SceneManager.LoadScene("CombatScene",LoadSceneMode.Additive);
        level_content.SetActive(false);
    }

    public void ReturnToLevel()
    {
        SceneManager.UnloadSceneAsync("CombatScene");
        if (score == 4)
        {
            SceneManager.LoadScene("WinScreen");
        }
        else
        {
            SceneManager.SetActiveScene(level);
            level_content.SetActive(true);
            score++;
        }
    }

    public void LoseGame()
    {
        SceneManager.UnloadSceneAsync("CombatScene");
        SceneManager.LoadScene("DeathScreen");
    }
}
