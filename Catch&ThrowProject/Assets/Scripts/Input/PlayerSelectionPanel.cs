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

    public SkinnedMeshRenderer dummyMesh;
    public SkinnedMeshRenderer maskMesh;

    public Material[] playerTextures;
    public Material[] maskTextures;

    public Material playerMaterial;
    public Material maskMaterial;

    public GameObject buttonPanel;
    public GameObject readyPanel;

    public GameObject playerVisualComponent;

    public void Recolor(int index)
    {
        dummyMesh.material = playerTextures[index];
        maskMesh.material = maskTextures[index];

        playerMaterial = dummyMesh.sharedMaterial;
        maskMaterial = maskMesh.sharedMaterial;
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
