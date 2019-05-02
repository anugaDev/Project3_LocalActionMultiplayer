using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerSelectionPanel : MonoBehaviour
{
    public GameObject playerPrefab;

    public bool HasPlayer = false;

    public int controllerNumber;

    public StandaloneInputModule panelInput;

    public Image parentPanel;
    public Text pressToAssignText;

    public void AssignController(int controllerNumber)
    {
        parentPanel.enabled = false;
        pressToAssignText.enabled = false;

        HasPlayer = true;
        this.controllerNumber = controllerNumber;

        SetPanelInput(controllerNumber);

        gameObject.SetActive(true);
    }

    public void CreatePlayer(int playerNumber)
    {
        PlayerController newPlayer = Instantiate(playerPrefab.gameObject).GetComponent<PlayerController>();

        newPlayer.inputControl.controllerNumber = playerNumber;
    }

    private void SetPanelInput(int playerNumber)
    {
        panelInput.horizontalAxis = "Horizontal" + playerNumber;
        panelInput.verticalAxis = "Vertical" + playerNumber;
        panelInput.submitButton = "Fire" + playerNumber;
        panelInput.cancelButton = "Jump" + playerNumber;

        panelInput.enabled = true;
    }
}
