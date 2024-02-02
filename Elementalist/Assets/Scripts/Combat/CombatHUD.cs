using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatHUD : MonoBehaviour
{
    public TMP_Text DialogueText; 
    public Slider PlayerHPSlider;
    public Slider EnemyHPSlider;
    public Slider MPSlider;

    public void Start()
    {
        PlayerHPSlider.maxValue = 100;
        EnemyHPSlider.maxValue = 40;
        MPSlider.maxValue = 50;
    }

    public void setPlayerHP(int hp)
    {
       PlayerHPSlider.value = hp;
    }

    public void setEnemyHP(int hp)
    {
        EnemyHPSlider.value = hp;
    }

    public void setMP(int mp)
    {
        MPSlider.value = mp;
    }
}
