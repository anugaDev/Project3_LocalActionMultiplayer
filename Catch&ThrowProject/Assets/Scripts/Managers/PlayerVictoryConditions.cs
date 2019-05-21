using System.Collections;
using System.Collections.Generic;
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
            var kills = 0;
            var playerToRank = new PlayerController();

            foreach (KeyValuePair<PlayerController, PlayerMatchInfo> player in matchInfo)
            {
                if (player.Value.kills > kills && player.Value.rank == 0)
                {
                    playerToRank = player.Key;
                    kills = player.Value.kills;
                }
            }

            matchInfo[playerToRank].rank = i;
        }
    }

    public void UpdateValues(PlayerController player)
    {
        matchInfo[player].deaths++;

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
