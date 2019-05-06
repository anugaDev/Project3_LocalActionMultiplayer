using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelNavigation : MonoBehaviour
{
    public PlayerSelectionPanel playerPanel;

    public List<Button> selectableButtons;
    public Button currentButton;

    public string SubmitButton;
    public string CancelButton;

    public bool CanNavigate;

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private void Start()
    {
        currentButton = selectableButtons[0];
    }

    private void Update()
    {
        if (Input.GetButtonDown(SubmitButton + playerPanel.controllerNumber)) playerPanel.ReadyCheck(true);
        if (Input.GetButtonDown(CancelButton + playerPanel.controllerNumber)) playerPanel.ReadyCheck(false);

        if (CanNavigate)
        {
            if (Input.GetAxis("Vertical" + playerPanel.controllerNumber) > 0) Move(Direction.Up);
            if (Input.GetAxis("Vertical" + playerPanel.controllerNumber) < 0) Move(Direction.Down);
        }
        else if (Input.GetAxis("Vertical" + playerPanel.controllerNumber) == 0) CanNavigate = true;
    }

    private void Move(Direction direction)
    {
        CanNavigate = false;

        switch (direction)
        {
            case Direction.Up:
                if (selectableButtons.IndexOf(currentButton) != 0)
                    currentButton = selectableButtons[selectableButtons.IndexOf(currentButton) - 1];
                break;
            case Direction.Down:
                if (selectableButtons.IndexOf(currentButton) != selectableButtons.Count - 1)
                    currentButton = selectableButtons[selectableButtons.IndexOf(currentButton) + 1];
                break;
        }

        currentButton.onClick.Invoke();
    }
}
