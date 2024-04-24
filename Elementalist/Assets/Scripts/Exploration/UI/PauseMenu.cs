using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private Exploration_HUD HUD;

    public GameObject OptionsPanel, SettingsPanel;

    public Slider VolumeSlider, SensitivitySlider;

    private CinemachineFreeLook FreeLookCamera;

    private void Start()
    {
        HUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<Exploration_HUD>();
        FreeLookCamera = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().FreeLookCamera;
        SensitivitySlider.value = FreeLookCamera.m_XAxis.m_MaxSpeed / 100;
    }

    public void OnSettingsButton()
    {
        OptionsPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }

    public void OnSaveGameButton()
    {
        GameManager.Instance.SaveGame();
    }

    public void OnLoadGameButton()
    {
        HUD.HidePauseMenu();
        GameManager.Instance.StartLoadGame();
    }

    public void OnMainMenuButton()
    {
        HUD.HidePauseMenu();
        GameManager.Instance.LoadMainMenu();
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnCloseSettingsButton()
    {
        SettingsPanel.SetActive(false);
        OptionsPanel.SetActive(true);
    }

    public void OnSensitivityValueChange()
    {
        float value = SensitivitySlider.value;
        
        FreeLookCamera = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().FreeLookCamera;
        FreeLookCamera.m_XAxis.m_MaxSpeed = value * 50;
    }
}
