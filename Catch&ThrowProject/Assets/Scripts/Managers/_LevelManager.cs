using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class _LevelManager : MonoBehaviour
{
    public static _LevelManager instance;

    public List<Transform> spawnPoints;
    public List<Transform> ammoSpawnPoints;
    public List<PlayerController> players;

    public StandaloneInputModule UI_Input;

    public DynamicCamera cameraFollow;

    public PlayerVictoryConditions matchInfo = new PlayerVictoryConditions();

    [Header("Match Settings")]
    public int StartingLifes = 10;
    public int MatchDuration = 120;
    [SerializeField] private float timeForAmmo;

    public bool endlessGame = false;
    public bool matchByTime = false;
    private float gameTimer = 0f;
    public Text remainingTime;
    public GameObject remainingTimeParent;

    public int timeToChangeScene = 3;
    public float focusPlayerZoom = 30;

    [Header("Ammo Count Settings")]
    [SerializeField] private GameObject ammoItem;
    [HideInInspector] public float scatteredAmmo;
    private float playersInitialAmmo;
    private float matchAmmo;

    [Header("UI Elements")]
    public GameObject UI_Parent;
    public GameObject PauseMenu;

    public GameObject countdownText;

    public bool testingScene;
    public bool tutorialScene = false;
    
    [Header("Level Sound")]
    
    [FMODUnity.EventRef] public string levelMusic;
    [HideInInspector] public FMOD.Studio.EventInstance musicEvent;
    
    [FMODUnity.EventRef] public string endingSound;
    

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

        if (_GameManager.instance != null)
        {
            if (!tutorialScene) matchByTime = _GameManager.instance.gameByTime;
        }
        
        if (levelMusic != "") musicEvent = FMODUnity.RuntimeManager.CreateInstance(levelMusic);
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

        if (tutorialScene)
        {
            _GameManager.instance.SceneToLoadNumber++;
            _GameManager.instance.playerControllers = players;
        }
    }

    private void Update()
    {
        if (matchByTime && matchState == MatchState.Playing)
        {
            gameTimer -= Time.deltaTime;
            remainingTime.text = ((int)gameTimer).ToString();

            if (gameTimer <= 0)
            {
                gameTimer = 0;

                StartCoroutine(EndMatch());
            }
        }

        CheckForAmmoSpawn();
    }

    private void StartGame()
    {
        musicEvent.start();
        if (matchByTime)
        {
            gameTimer = MatchDuration;
            remainingTime.enabled = true;
            remainingTimeParent.SetActive(true);
        }

        cameraFollow.enabled = true;

        playersInitialAmmo = players[0].GetInitialAmmo();
        for (int i = 0; i < players.Count; i++)
        {
            players[i].health = StartingLifes;
            players[i].uiPanel = Instantiate(players[i].playerSkin.UI_Player, UI_Parent.transform).GetComponent<UpdatePlayerPanel>();
            players[i].uiPanel.SetUpPanel(matchByTime, players[i].playerNumber);
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
            if (!matchByTime && !endlessGame)
            {
                cameraFollow.objectsToShow.Remove(player.transform);
                player.gameObject.SetActive(false);

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

                    StartCoroutine(EndMatch());
                }
            }
        }
    }

    private IEnumerator EndMatch()
    {
        matchState = MatchState.Ending;

        if (tutorialScene)
        {
            _GameManager.instance.StartCoroutine(_GameManager.instance.LoadNewGame());

            Destroy(gameObject);
        }
        else
        {
            musicEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            RuntimeManager.PlayOneShot(endingSound);
            _GameManager.instance.playerControllers.Clear();
            if (matchByTime) matchInfo.SetRankingsByKills();

            cameraFollow.objectsToShow.Clear();
            cameraFollow.objectsToShow.Add(matchInfo.GetWinner().transform);
            cameraFollow.minZoom = focusPlayerZoom;
            cameraFollow.positionDamping = 0f;
            matchInfo.GetWinner().gameObject.SetActive(true);
            matchInfo.GetWinner().rigidbody.velocity = Vector3.zero;
            matchInfo.GetWinner().rigidbody.isKinematic = true;

            yield return new WaitForSeconds(timeToChangeScene);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
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

    private void CheckForAmmoSpawn()
    {
        matchAmmo = 0;
        foreach (var player in players)
        {
            matchAmmo += player.actualAmmo;
        }

        matchAmmo += scatteredAmmo;

        if (matchAmmo < (players.Count * playersInitialAmmo))
        {
            StartCoroutine(SpawnAmmo(timeForAmmo));
        }
    }

    private IEnumerator SpawnAmmo(float time)
    {
        scatteredAmmo += 1;
        yield return new WaitForSeconds(time);
        Instantiate(ammoItem, ammoSpawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)].transform);
    }
}
