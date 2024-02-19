using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public void RestartGame()
    {
        GameManager.Instance.LoadLevel();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
