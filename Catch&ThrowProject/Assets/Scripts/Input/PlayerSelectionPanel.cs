using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerSelectionPanel : MonoBehaviour
{
    public GameObject playerPrefab;

    public bool HasPlayer = false;
    public bool GameStarting = false;

    public int controllerNumber;

    public Image parentPanel;
    public Text pressToAssignText;

    public MeshRenderer meshRenderer;

    public int[] materialPositions;

    public GameObject buttonPanel;
    public GameObject readyPanel;

    public GameObject playerVisualComponent;

    public void ChangeColor(Material newMaterial)
    {
        var actualMaterials = meshRenderer.materials;

        for (int i = 0; i < actualMaterials.Length; i++)
        {
            actualMaterials[i] = newMaterial;
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
    }
}
