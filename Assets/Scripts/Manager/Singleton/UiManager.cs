using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager>
{
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject soundPanel;
    [SerializeField] private GameObject keyPanel;
    [SerializeField] private GameObject beforeMainPanel;
    public bool Pause=false;

    protected override void Awake()
    {
        base.Awake();
        settingPanel.SetActive(false);
        soundPanel.SetActive(false);
        keyPanel.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (soundPanel.activeSelf)
            {
                CloseSoundPanel();
            }
            else if (keyPanel.activeSelf) 
            {
                CloseKeyPanel();
            }
            else if(beforeMainPanel.activeSelf)
            {
                CloseBeforeMainPanel();
            }
            else
            {
                settingPanel.SetActive(!settingPanel.activeSelf);
                Pause = !Pause;
                if (Pause)
                {
                    Time.timeScale = 0f;
                }
                else
                {
                    Time.timeScale = 1f;
                }
            }
        }
    }
    
    public void OpenSetting()
    {
        settingPanel.SetActive(true);
    }
    public void CloseSetting()
    {
        settingPanel.SetActive(false);
        Pause = false;
        Time.timeScale = 1;
    }



    public void OpenKeyPanel()
    {
        keyPanel.SetActive(true);
    }
    public void CloseKeyPanel()
    {
        keyPanel.SetActive(false);
    }

    public void OpenBeforeMainPanel()
    {
        beforeMainPanel.SetActive(true);
    }
    public void CloseBeforeMainPanel()
    {
        beforeMainPanel.SetActive(false);
    }

    public void ContinueGame()
    {
        settingPanel.SetActive(false);
        Pause = false;
        Time.timeScale = 1;
    }

    public void OpenSoundPanel()
    {
        soundPanel.SetActive(true);
    }
    public void CloseSoundPanel()
    {
        soundPanel.SetActive(false);
    }
    
    
    public void MainMenu()
    {
        CloseBeforeMainPanel();
        CloseSetting();
        SceneManager.LoadScene("LobbyScene");
    }
}
