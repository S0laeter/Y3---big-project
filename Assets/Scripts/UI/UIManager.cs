using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private SceneTransition sceneTransition;

    public GameObject winMessage;
    public GameObject loseMessage;
    public GameObject pauseMenu;

    private void OnEnable()
    {
        //subscribing to actions
        Actions.Win += Win;
        Actions.Lose += Lose;
    }

    private void OnDisable()
    {
        //unsubscribing to actions
        Actions.Win -= Win;
        Actions.Lose -= Lose;
    }

    // Start is called before the first frame update
    void Start()
    {
        sceneTransition = GetComponentInChildren<SceneTransition>();

        StartCoroutine(StartUpTutorial());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            PauseButton();
        }

    }

    public void Win()
    {
        winMessage.SetActive(true);
        loseMessage.SetActive(false);
    }
    public void Lose()
    {
        loseMessage.SetActive(true);
        winMessage.SetActive(false);
    }



    public void PauseButton()
    {
        if (pauseMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
        else if (!pauseMenu.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    public void RestartButton()
    {
        Time.timeScale = 1f;
        sceneTransition.LoadSceneAfterTransition("Level1");
    }
    public void QuitButton()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }


    private IEnumerator StartUpTutorial()
    {
        yield return new WaitForSeconds(1.01f);

        PauseButton();
    }

}
