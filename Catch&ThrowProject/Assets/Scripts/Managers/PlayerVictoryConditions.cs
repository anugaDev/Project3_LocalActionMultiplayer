using System.Collections;
using System.Collections.Generic;
using Assets.Resources;
using UnityEngine;

public class PlayerVictoryConditions
{
    public Dictionary<PlayerController, PlayerMatchInfo> matchInfo = new Dictionary<PlayerController, PlayerMatchInfo>();

    public void SetPlayer(PlayerController player)
    {
        PlayerMatchInfo newPlayerInfo = new PlayerMatchInfo();

        newPlayerInfo.kills = 0;
        newPlayerInfo.deaths = 0;
        newPlayerInfo.suicides = 0;
        newPlayerInfo.skin = player.playerSkin;

        matchInfo.Add(player, newPlayerInfo);
    }

    public void SetRankingsByKills()
    {
        for (int i = 1; i <= _LevelManager.instance.players.Count; i++)
        {
            int kills = 0;
            PlayerController playerToRank = _LevelManager.instance.players[0];

            foreach (PlayerController player in _LevelManager.instance.players)
            {
                if (matchInfo[player].kills >= kills && matchInfo[player].rank == 0)
                {
                    playerToRank = player;
                    kills = matchInfo[player].kills;
                }
            }

            matchInfo[playerToRank].rank = i;
        }
    }

    public void UpdateValues(PlayerController player)
    {
        matchInfo[player].deaths++;
        Debug.Log(matchInfo[player].deaths);
        if (player.killer) matchInfo[player.killer].kills++;
        else matchInfo[player].suicides++;
    }
}

public class PlayerMatchInfo
{
    public int rank = 0;
    public int kills;
    public int deaths;
    public int suicides;

    public Skin skin;
}
