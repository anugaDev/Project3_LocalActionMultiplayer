using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class _LevelManager : MonoBehaviour
{
    public static _LevelManager instance;

    public List<Transform> spawnPoints;
    public List<PlayerController> players;

    public StandaloneInputModule UI_Input;

    public DynamicCamera cameraFollow;

    [Header("UI Elements")]
    public GameObject UI_Parent;
    public GameObject PauseMenu;
    public GameObject playerPanelPrefab;

    public enum MatchState
    {
        Starting,
        Playing,
        Ending,
        Paused
    }

    public MatchState matchState;

    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(this);
    }

    public void SetNewGame()
    {
        matchState = MatchState.Starting;

        for (int i = 0; i < players.Count; i++)
        {
            SpawnPlayer(players[i].gameObject, i);

            cameraFollow.objectsToShow.Add(players[i].transform);
        }

        //Here you would wait until counter goes down or some visual effect

        StartGame();
    }

    private void StartGame()
    {
        cameraFollow.enabled = true;

        for (int i = 0; i < players.Count; i++)
        {
            players[i].uiPanel = Instantiate(playerPanelPrefab, parent: UI_Parent.transform).GetComponent<UpdatePlayerPanel>();
            players[i].enabled = true;
        }

        matchState = MatchState.Playing;
    }

    public void SpawnPlayer(GameObject player, int? position)
    {
        Vector3 spawnPosition;

        spawnPosition = position.HasValue ? spawnPoints[position.Value].position :
                                            spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count - 1)].position;

        player.transform.position = spawnPosition;
    }

    public void PauseGame(int playerNumber)
    {
        matchState = MatchState.Paused;

        SetInputUI(playerNumber);

        PauseMenu.SetActive(true);

        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        PauseMenu.SetActive(false);

        Time.timeScale = 1;

        matchState = MatchState.Playing;
    }

    private void SetInputUI(int playerNumber)
    {
        UI_Input.horizontalAxis = "Horizontal" + playerNumber;
        UI_Input.verticalAxis = "Vertical" + playerNumber;

        UI_Input.submitButton = "Jump" + playerNumber;
        UI_Input.cancelButton = "Fire" + playerNumber;
    }
}
