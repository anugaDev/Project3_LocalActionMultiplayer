using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private int MainMenuSceneIndex;

    public void ResumeGame()
    {
        _LevelManager.instance.ResumeGame();
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(MainMenuSceneIndex);
    }

    public void ResetGame()
    {
        _LevelManager.instance.ResetGame();
    }

    //Implement GameSettings Methods
}
