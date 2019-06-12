using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePlayerPanel : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Image[] lifes;

    [Header("Text")]
    [SerializeField] private Text kills;
    [SerializeField] private Text PlayerNumber;

    public void SetUpPanel(bool gameByTime, int playerNumber)
    {
        PlayerNumber.text = "P" + playerNumber;
        kills.text = "0";
        kills.enabled = gameByTime;

        for (int i = 0; i < lifes.Length; i++)
        {
            lifes[i].enabled = !gameByTime;
        }
    }

    public void UpdateKills(int actualKills)
    {
        kills.text = actualKills.ToString();
    }

    public void RemoveLife(int actualHealth)
    {
        if (_LevelManager.instance.matchByTime) return;

        for (int i = lifes.Length - 1; i >= actualHealth; i--)
        {
            if (lifes[i]) lifes[i].enabled = false;
        }
    }
}
