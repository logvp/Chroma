using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiUtils : MonoBehaviour
{
    public static void loadMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public static void loadGame()
    {
        SceneManager.LoadScene("Demo");
    }

    public static void quitTheWholeGame()
    {
        Application.Quit();
    }
}
