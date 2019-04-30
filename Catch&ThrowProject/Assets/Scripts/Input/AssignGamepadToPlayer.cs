using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignGamepadToPlayer : MonoBehaviour
{
    public int maxPlayers = 4;

    public string joinButton;

    public PlayerSelectionPanel[] playerSelectionPanels;

    void Start()
    {

    }

    void Update()
    {
        for (int i = 0; i < maxPlayers; i++)
        {
            if (Input.GetButtonDown(joinButton + i))
            {
                AddPlayerController(i);
            }
        }
    }

    private void AddPlayerController(int controllerNumber)
    {
        for (int i = 0; i < playerSelectionPanels.Length; i++)
        {
            if (playerSelectionPanels[i].HasPlayer == false)
            {
                playerSelectionPanels[i].CreatePlayer(controllerNumber);
                break;
            }
        }
    }
}
