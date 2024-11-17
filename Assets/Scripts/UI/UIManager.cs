using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private SceneTransition sceneTransition;

    private GameObject settingsMenu;
    private Vector3 settingsMenuOffscreenPos;

    // Start is called before the first frame update
    void Start()
    {
        sceneTransition = GetComponentInChildren<SceneTransition>();

        settingsMenu = GameObject.Find("SettingsMenu");
        if (settingsMenu != null )
            settingsMenuOffscreenPos = settingsMenu.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButton()
    {
        Time.timeScale = 1f;
        sceneTransition.LoadSceneAfterTransition("SelectLevel");
    }
    
    public void LoadLevel1()
    {
        Time.timeScale = 1f;
        sceneTransition.LoadSceneAfterTransition("Level1");
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1f;
        sceneTransition.LoadSceneAfterTransition("MainMenu");
    }

    public void SettingsMenuButton(bool toggleOn)
    {
        if (toggleOn)
            settingsMenu.transform.position = settingsMenuOffscreenPos + Vector3.down * 2000;
        else
            settingsMenu.transform.position = settingsMenuOffscreenPos;
    }

    public void QuitButton()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

}
