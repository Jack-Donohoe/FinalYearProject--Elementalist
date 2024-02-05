using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Scene level;
    private Scene combat;

    private GameObject level_content;
    
    private static GameManager instance;

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

        level = SceneManager.GetSceneByName("TestScene");
        combat = SceneManager.GetSceneByName("CombatScene");

        level_content = GameObject.Find("Level");
        score = 0;
    }
    
    public void LoadLevel()
    {
        SceneManager.LoadScene("TestScene");
    }
    
    public void StartCombat()
    {
        SceneManager.LoadScene("CombatScene",LoadSceneMode.Additive);
        level_content.SetActive(false);
    }

    public void ReturnToLevel()
    {
        if (score == 4)
        {
            SceneManager.LoadScene("WinScreen");
        }
        else
        {
            SceneManager.SetActiveScene(level);
            level_content.SetActive(true);
            SceneManager.UnloadSceneAsync("CombatScene");
            score++;
        }
    }

    public void LoseGame()
    {
        SceneManager.LoadScene("DeathScreen");
    }
}
