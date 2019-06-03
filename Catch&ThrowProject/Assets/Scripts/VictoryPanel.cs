using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryPanel : MonoBehaviour
{
    public PlayerController player;
    public Text rank;
    public Text kills;
    public Text deaths;
    public Text suicides;

    public void SetValues(PlayerMatchInfo player)
    {
        rank.text = "Rank: " + player.rank;
        kills.text = "Kills: " + player.kills;
        deaths.text = "Deaths: " + player.deaths;
        suicides.text = "Suicides: " + player.suicides;
    }
}
