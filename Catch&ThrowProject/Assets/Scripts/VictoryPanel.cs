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

    public Image border;
    public Image background;

    public void SetValues(PlayerMatchInfo player)
    {
        rank.text = "Rank: " + player.rank;
        kills.text = "Kills: " + player.kills;
        deaths.text = "Deaths: " + player.deaths;
        suicides.text = "Suicides: " + player.suicides;

        //rank.color = player.skin.mainColor;
        //kills.color = player.skin.mainColor;
        //deaths.color = player.skin.mainColor;
        //suicides.color = player.skin.mainColor;

        border.color = player.skin.mainColor;
        background.color = player.skin.mainColor - new Color(0, 0, 0, 0.5f);
    }
}
