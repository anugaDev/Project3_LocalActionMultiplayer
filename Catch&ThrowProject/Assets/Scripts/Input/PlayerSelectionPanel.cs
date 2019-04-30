using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionPanel : MonoBehaviour
{
    public GameObject playerPrefab;

    public bool HasPlayer { get; private set; }

    void Start()
    {

    }

    void Update()
    {

    }

    public void CreatePlayer(int playerNumber)
    {
        PlayerController newPlayer = Instantiate(playerPrefab.gameObject).GetComponent<PlayerController>();

        newPlayer.inputControl.controllerNumber = playerNumber;
    }
}
