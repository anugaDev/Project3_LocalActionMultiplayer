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

    public MeshRenderer dummyMesh;
    public MeshRenderer maskMesh;

    public Material dummyMat;
    public Material maskMat;

    public int[] materialPositions;

    public GameObject buttonPanel;
    public GameObject readyPanel;

    public GameObject playerVisualComponent;

    public void Recolor(MeshRenderer[] meshes)
    {
        dummyMesh.material = meshes[0].material;
        maskMesh.material = meshes[1].material;

        dummyMat = meshes[0].material;
        maskMat = meshes[1].material;
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
