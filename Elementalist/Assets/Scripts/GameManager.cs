using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Scene level;
    private Scene combat;
    
    private static GameManager instance;

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
    }
    
    public void LoadLevel()
    {
        SceneManager.LoadScene("TestScene");
    }
    
    public void StartCombat()
    {
        SceneManager.LoadScene("CombatScene",LoadSceneMode.Additive);
    }

    public void ReturnToLevel()
    {
        SceneManager.SetActiveScene(level);
        SceneManager.UnloadSceneAsync(combat);
    }
}
