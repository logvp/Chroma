using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    private static string gameSceneName = "Demo";
    // zero based index
    private static int levelSelected = 0;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public static void StartLevelOne()
    {
        StartLevel(0);
    }

    public static void StartLevel(int level)
    {
        levelSelected = level;
        SceneManager.LoadScene(gameSceneName);
    }

    // deprecated but I don't care
    void OnLevelWasLoaded(int sceneId)
    {
        if (sceneId == 0) return;
        GameState.Player.transform.position = GameState.Checkpoints[levelSelected].position;
    }
}
