using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignGamepadToPlayer : MonoBehaviour
{
    public int maxPlayers = 4;

    public string joinButton = "Fire";

    public PlayerSelectionPanel[] playerSelectionPanels;

    void Start()
    {

    }

    void Update()
    {
        for (int i = 1; i <= maxPlayers; i++)
        {
            if (Input.GetButtonDown(joinButton + i) && !IsControllerAssigned(i))
            {
                AddPlayerController(i);
            }
        }
    }

    private void AddPlayerController(int controller)
    {
        for (int i = 0; i < playerSelectionPanels.Length; i++)
        {
            if (playerSelectionPanels[i].HasPlayer == false)
            {
                playerSelectionPanels[i].AssignController(controller);
                break;
            }
        }
    }

    private bool IsControllerAssigned(int controllerNumber)
    {
        for (int i = 0; i < playerSelectionPanels.Length; i++)
        {
            if (playerSelectionPanels[i].controllerNumber == controllerNumber) return true;
        }

        return false;
    }
}
