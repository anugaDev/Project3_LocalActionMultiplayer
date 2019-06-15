using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class ChangePlayerMaterial : MonoBehaviour
{
    public PlayerSelectionPanel playerPanel;

    public int currentIndex;

    public string SubmitButton;
    public string CancelButton;

    public string left;
    public string right;

    [FMODUnity.EventRef] public string selectPlayer;
    [FMODUnity.EventRef] public string cancelPlayer;
    [FMODUnity.EventRef] public string changePlayer;

    public enum InputDirection
    {
        Left,
        Right
    }

    private void Start()
    {
        currentIndex = 0;

        playerPanel.Recolor(currentIndex);
    }

    private void Update()
    {
        if (playerPanel.GameStarting) return;

        if (Input.GetButtonDown(SubmitButton + playerPanel.controllerNumber))
        {
            playerPanel.ReadyCheck(true);
            RuntimeManager.PlayOneShot(selectPlayer);
        }

        if (Input.GetButtonDown(CancelButton + playerPanel.controllerNumber))
        {
            playerPanel.ReadyCheck(false);
        }

        if (playerPanel.readyPanel.gameObject.activeSelf) return;

        if (Input.GetButtonDown(left + playerPanel.controllerNumber)) playerPanel.Recolor(GetMesh(InputDirection.Left));
        if (Input.GetButtonDown(right + playerPanel.controllerNumber)) playerPanel.Recolor(GetMesh(InputDirection.Right));
    }

    private int GetMesh(InputDirection? direction)
    {
        switch (direction)
        {
            case InputDirection.Left:
                if (currentIndex == 0) currentIndex = playerPanel.availableSkins.Count - 1;
                else currentIndex--;
                break;
            case InputDirection.Right:
                if (currentIndex == playerPanel.availableSkins.Count - 1) currentIndex = 0;
                else currentIndex++;
                break;
        }
        RuntimeManager.PlayOneShot(changePlayer);


        return currentIndex;
    }
}
