using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Resources;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssignGamepadToPlayer : MonoBehaviour
{
    public int maxPlayers = 4;

    public string joinButton = "Fire";
    public string startGameButton = "Start";

    public PlayerSelectionPanel[] playerSelectionPanels;

    void Start()
    {
        foreach (Skin skin in playerSelectionPanels[0].availableSkins)
        {
            skin.used = false;
        }
    }

    void Update()
    {
        for (int i = 1; i <= maxPlayers; i++)
        {
            if (Input.GetButtonDown(joinButton + i) && !IsControllerAssigned(i))
            {
                AddPlayerController(i);
            }

            if (Input.GetButtonDown(startGameButton + i))
            {
                if (CanStartGame())
                {
                    _GameManager.instance.SceneToLoadNumber = SceneManager.GetActiveScene().buildIndex + 1;
                    _GameManager.instance.StartCoroutine(_GameManager.instance.LoadNewGame());
                }
            }
        }
    }

    public bool CanStartGame()
    {
        _GameManager.instance.players.Clear();

        int amountOfPlayers = 0;
        int amountOfControllersConnected = 0;

        for (int i = 0; i < 4; i++)
        {
            if (playerSelectionPanels[i].HasPlayer)
            {
                if (playerSelectionPanels[i].readyPanel.gameObject.activeSelf)
                {
                    print("Added");
                    _GameManager.instance.players.Add(playerSelectionPanels[i]);

                    amountOfPlayers++;
                }

                amountOfControllersConnected++;
            }
        }

        if (amountOfPlayers < 1 || amountOfPlayers != amountOfControllersConnected) return false;

        return true;
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
