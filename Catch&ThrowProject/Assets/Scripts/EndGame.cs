using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EndGame : MonoBehaviour
{
    public PlayerMatchInfo[] ranking;

    public List<GameObject> players;

    public List<VictoryPanel> victoryPanels;

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
        _GameManager.instance.GoToMainMenu();
    }
}
