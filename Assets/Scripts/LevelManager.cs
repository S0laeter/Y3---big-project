using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
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
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        //auto invisible cursor in this "locked" state
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else if (Cursor.lockState == CursorLockMode.None)
                Cursor.lockState = CursorLockMode.Locked;
        }

    }

    public void Win()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void Lose()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
