﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class _LevelManager : MonoBehaviour
{
    public static _LevelManager instance;

    public List<Transform> spawnPoints;
    public List<PlayerController> players;

    public StandaloneInputModule UI_Input;

    public DynamicCamera cameraFollow;

    public PlayerVictoryConditions matchInfo = new PlayerVictoryConditions();

    [Header("Match Settings")]
    public int StartingLifes = 10;
    public int MatchDuration = 120;

    public bool matchByTime = false;
    private float gameTimer = 0f;

    [Header("UI Elements")]
    public GameObject UI_Parent;
    public GameObject PauseMenu;
    public GameObject EndMenu;
    public GameObject playerPanelPrefab;

    public GameObject countdownText;

    public bool testingScene;

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

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        if (testingScene)
        {
            var playerPrefs = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in playerPrefs)
            {
                players.Add(player.GetComponent<PlayerController>());
                if (!player.activeSelf) player.SetActive(true);
            }

            CheckTest();
            SetNewGame();
        }
    }

    private void Update()
    {
        if (matchByTime && matchState == MatchState.Playing)
        {
            gameTimer += Time.deltaTime;

            if (gameTimer >= MatchDuration)
            {
                gameTimer = 0;

                EndMatch();
            }
        }
    }

    private void StartGame()
    {
        cameraFollow.enabled = true;

        for (int i = 0; i < players.Count; i++)
        {
            players[i].health = StartingLifes;
            players[i].uiPanel = Instantiate(playerPanelPrefab, parent: UI_Parent.transform).GetComponent<UpdatePlayerPanel>();
            players[i].uiPanel.RemoveLife(players[i].health);
            players[i].enabled = true;
        }

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

    public void SetNewGame()
    {
        matchState = MatchState.Starting;

        for (int i = 0; i < players.Count; i++)
        {
            SpawnPlayer(players[i].gameObject, i);
            players[i].enabled = false;
            matchInfo.SetPlayer(players[i]);

            cameraFollow.objectsToShow.Add(players[i].transform);
        }

        if (testingScene) StartGame();
        else StartCoroutine(StartCountdown(3));
    }

    public void SpawnPlayer(GameObject player, int? position)
    {
        if (spawnPoints.Count == 0) return;

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

    public void ResetGame()
    {

    }

    public void OnPlayerKilled(PlayerController player)
    {
        if (player.health == 0) return;

        player.health--;
        player.uiPanel.RemoveLife(player.health);
        matchInfo.UpdateValues(player);

        if (player.health == 0)
        {
            cameraFollow.objectsToShow.Remove(player.transform);
            player.gameObject.SetActive(false);

            if (!matchByTime)
            {
                int alivePlayers = 0;

                for (int i = 0; i < players.Count; i++)
                {
                    if (players[i].health > 0) alivePlayers++;
                }

                matchInfo.matchInfo[player].rank = alivePlayers + 1;

                if (alivePlayers == 1)
                {
                    for (int i = 0; i < players.Count; i++)
                    {
                        if (players[i].health > 0) matchInfo.matchInfo[players[i]].rank = 1;
                    }

                    EndMatch();
                }
            }
        }
    }

    private void EndMatch()
    {
        matchState = MatchState.Ending;

        if (matchByTime) matchInfo.SetRankingsByKills();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public PlayerMatchInfo[] PassRanking()
    {
        PlayerMatchInfo[] ranking = new PlayerMatchInfo[players.Count];

        foreach (PlayerController player in players)
        {
            ranking[matchInfo.matchInfo[player].rank - 1] = matchInfo.matchInfo[player];
        }

        return ranking;
    }

    public void CheckTest()
    {
        players.Clear();

        var playerPrefs = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in playerPrefs)
        {
            players.Add(player.GetComponent<PlayerController>());
            if (!player.activeSelf) player.SetActive(true);
        }
    }

    public IEnumerator StartCountdown(int secondsBeforeGame)
    {
        Time.timeScale = 0;

        countdownText.SetActive(true);

        Text backgroundText = countdownText.GetComponent<Text>();
        Text foregroundText = countdownText.transform.GetChild(0).GetComponent<Text>();

        for (int i = secondsBeforeGame; i > 0; i--)
        {
            backgroundText.text = i.ToString();
            foregroundText.text = i.ToString();

            yield return new WaitForSecondsRealtime(1);
        }

        backgroundText.text = "Blitz!";
        foregroundText.text = "Blitz!";

        yield return new WaitForSecondsRealtime(1);

        countdownText.SetActive(false);

        StartGame();
    }
}
