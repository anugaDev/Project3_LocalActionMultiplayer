using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndGame : MonoBehaviour
{
    public PlayerMatchInfo[] ranking;

    public List<GameObject> players;

    public List<VictoryPanel> victoryPanels;

    public string StartButton = "Start";

    void Update()
    {
        for (int i = 1; i <= 5; i++)
        {
            if (Input.GetButtonDown(StartButton + i)) GoEnd();
        }
    }

    void Start()
    {
        if (!_LevelManager.instance) return;

        ranking = _LevelManager.instance.PassRanking();

        Destroy(_LevelManager.instance.gameObject);

        SetVictoryScreen();
    }

    private void SetVictoryScreen()
    {
        players = new List<GameObject>();

        for (int i = 0; i < ranking.Length; i++)
        {
            victoryPanels[i].SetValues(ranking[i]);
            //Do animation
            victoryPanels[i].player.SetSkin(ranking[i].skin);

            victoryPanels[i].gameObject.SetActive(true);
        }
    }

    public void GoEnd()
    {
        Time.timeScale = 1;
        Destroy(_GameManager.instance.gameObject);
        SceneManager.LoadScene(0);
    }
}
