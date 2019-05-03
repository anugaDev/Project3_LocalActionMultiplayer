﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerSelectionPanel : MonoBehaviour
{
    public GameObject playerPrefab;

    public bool HasPlayer = false;

    public int controllerNumber;

    public Image parentPanel;
    public Image selectionImage;
    public Text pressToAssignText;

    public MeshRenderer meshRenderer;

    public int[] materialPositions;

    public GameObject buttonPanel;
    public GameObject readyPanel;

    private void CreatePlayer(int playerNumber)
    {
        PlayerController newPlayer = Instantiate(playerPrefab.gameObject).GetComponent<PlayerController>();

        newPlayer.inputControl.controllerNumber = playerNumber;
    }

    public void ChangeColor(Material newMaterial)
    {
        var actualMaterials = meshRenderer.materials;

        for (int i = 0; i < materialPositions.Length; i++)
        {
            actualMaterials[materialPositions[0]] = newMaterial;
        }

        meshRenderer.materials = actualMaterials;
    }

    public void AssignController(int controllerNumber)
    {
        parentPanel.enabled = false;
        pressToAssignText.enabled = false;

        HasPlayer = true;
        this.controllerNumber = controllerNumber;

        gameObject.SetActive(true);
    }

    public void ReadyCheck(bool ready)
    {
        buttonPanel.SetActive(!ready);
        readyPanel.SetActive(ready);
        selectionImage.enabled = !ready;
    }
}