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
        Input.ResetInputAxes();
        _LevelManager.instance.ResumeGame();
    }

    public void ExitGame()
    {
        Destroy(_LevelManager.instance.gameObject);
        if (_GameManager.instance) Destroy(_GameManager.instance.gameObject);
        Time.timeScale = 1;
        SceneManager.LoadScene(MainMenuSceneIndex);
    }

    public void ResetGame()
    {
        _LevelManager.instance.ResetGame();
    }

    //Implement GameSettings Methods
}
