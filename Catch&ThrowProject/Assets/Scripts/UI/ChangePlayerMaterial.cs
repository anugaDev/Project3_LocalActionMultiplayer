﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerMaterial : MonoBehaviour
{
    public PlayerSelectionPanel playerPanel;

    public List<MeshRenderer> dummyPresets;
    public List<MeshRenderer> maskPresets;
    public int currentIndex;

    public string SubmitButton;
    public string CancelButton;

    public string left;
    public string right;

    public enum InputDirection
    {
        Left,
        Right
    }

    private void Start()
    {
        currentIndex = 0;

        playerPanel.Recolor(GetMesh(null));
    }

    private void Update()
    {
        if (playerPanel.GameStarting) return;

        if (Input.GetButtonDown(SubmitButton + playerPanel.controllerNumber)) playerPanel.ReadyCheck(true);
        if (Input.GetButtonDown(CancelButton + playerPanel.controllerNumber)) playerPanel.ReadyCheck(false);

        if (playerPanel.readyPanel.gameObject.activeSelf) return;

        if (Input.GetButtonDown(left + playerPanel.controllerNumber)) playerPanel.Recolor(GetMesh(InputDirection.Left));
        if (Input.GetButtonDown(right + playerPanel.controllerNumber)) playerPanel.Recolor(GetMesh(InputDirection.Right));
    }

    private int GetMesh(InputDirection? direction)
    {
        switch (direction)
        {
            case InputDirection.Left:
                if (currentIndex == 0) currentIndex = dummyPresets.Count - 1;
                else currentIndex--;
                break;
            case InputDirection.Right:
                if (currentIndex == dummyPresets.Count - 1) currentIndex = 0;
                else currentIndex++;
                break;
        }

        return currentIndex;
    }
}
