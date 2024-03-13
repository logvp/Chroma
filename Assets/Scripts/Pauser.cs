using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pauser : MonoBehaviour
{
    public GameObject pauseScreen;

    void Start()
    {
        Unpause();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameState.isPaused)
            {
                Unpause();
            } else
            {
                Pause();
            }
        }
    }

    public void Unpause()
    {
        GameState.isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void Pause()
    {
        GameState.isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        pauseScreen.SetActive(true);
        Time.timeScale = 0;
    }
}
