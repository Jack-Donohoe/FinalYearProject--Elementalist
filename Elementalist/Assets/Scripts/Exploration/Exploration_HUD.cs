using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Exploration_HUD : MonoBehaviour
{
    public TMP_Text score_text;

    public void SetScoreText(string text)
    {
        score_text.text = text;
    }
}
