using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    public enum GameState {
        WaitingToStart,
        GamePlaying,
        GamePausedUI,
        GamePaused,
        GameOver,
    }

    private GameState state;
    private GameState lastStateBeforePause;

    private Player player;
    [SerializeField] private bool debugMode;
    [SerializeField] private GameDifficultyData[] gameDifficulties;
    [SerializeField] private int gameDifficultyIndex;

    private string currentTime;

    private void Awake() {
        //We check if there is already a Singleton of GameManager
        if (Instance != null && Instance != this) {
            Destroy(this);
            //throw new System.Exception($"[{this.name}] >>> An Instance of this Singleton already exists!");
        } else {
            //There are not instances
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start() {
        Cursor.visible = false;
        StarterAssetsInputs.OnEscapePressed += StarterAssetsInputs_OnEscapePressed;
        Diary.OnGamePausedUI += Diary_OnGamePausedUI;
        Diary.OnGameUnpausedUI += Diary_OnGameUnpausedUI;
        TryLoadSavedGameData();
        ChangeState(GameState.WaitingToStart);
        //ChangeState(GameState.GamePlaying);
    }

    private void Diary_OnGameUnpausedUI(object sender, EventArgs e) {
       ChangeState(GameState.GamePaused);
    }

    private void Diary_OnGamePausedUI(object sender, EventArgs e) {
        lastStateBeforePause = state;
        Cursor.visible = true;
        ChangeState(GameState.GamePausedUI);
    }

    private void StarterAssetsInputs_OnEscapePressed(object sender, EventArgs e) {
        if (IsGamePausedUI()) return;
        TogglePauseGame();
    }

    private void Update() {

        switch (state) {

            case GameState.WaitingToStart:
                //player is currently inside the cabin or still hasn't started the day
                break; 
            case GameState.GamePlaying:
                DayManager.Instance.UpdateCurrentState();
                currentTime = DayManager.Instance.GetCurrentGameTimeInHHMMSS();
                break;
            case GameState.GamePaused:
                //game is currently paused
                break;
            case GameState.GameOver:
                break;

        }

    }

    public void ChangeState(GameState state) {
        this.state = state;
    }

    public GameDifficultyData GetCurrentDifficultyData() {
        return gameDifficulties[gameDifficultyIndex];
    }

    public bool IsGamePlaying() {
        return state == GameState.GamePlaying;
    }

    public bool IsNight() {
        if(DayManager.Instance != null) {
            return DayManager.Instance.IsNight();
        } else {
            return false;
        }
    }

    public bool IsGamePaused() {
        return state == GameState.GamePaused;
    }

    public bool IsGamePausedUI() {
        return state == GameState.GamePausedUI;
    }

    private void TogglePauseGame() {
        if (!IsGamePaused()) {
            lastStateBeforePause = state;
            ChangeState(GameState.GamePaused);
            Cursor.visible = true;
            Time.timeScale = 0f;

            OnGamePaused?.Invoke(this, EventArgs.Empty);
        } else {
            ChangeState(lastStateBeforePause);
            Cursor.visible = false;
            Time.timeScale = 1f;

            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsInDebugMode() {
        return debugMode;
    }

    public Player GetPlayer() {
        return player;
    }

    public void SetPlayerOnSpawn(Player player) {
        this.player = player;
    }

    public void SetDifficulty(int difficultyIndex) {
        gameDifficultyIndex = difficultyIndex;
    }

    private void TryLoadSavedGameData() {
        if (SaveManager.TryReadSavedData<GameSaveData>("game_data",out GameSaveData gameData)) {
            gameDifficultyIndex = gameData.difficulty;
        }
    }

    private void OnDestroy() {
        StarterAssetsInputs.OnEscapePressed -= StarterAssetsInputs_OnEscapePressed;
    }

}

[System.Serializable]
public struct GameDifficultyData {

    public enum GameDifficultyLevel {
        Easy,
        Normal,
        Hard,
        Nightmare
    }

    [SerializeField] private GameDifficultyLevel difficultyLevel;
    [SerializeField] [Min(1)] [Tooltip("Max power level based for enemy spawning cap")] private int difficultyMaxPwrLvl;
    [SerializeField] [Min(15)] [Tooltip("Timer interval for new enemy spawning")] private int enemySpawnInterval;
    [SerializeField] [Min(1)] [Tooltip("Time speed multiplier")] private int difficultyDayTimeMultiplier;
    [SerializeField][Min(0.5f)][Tooltip("Difficulty multiplier used for enemy damage and more")] private float difficultyMultiplier;
    [SerializeField] [Min(1)] [Tooltip("Amout of sanity to drain every 'Sanity Debuff Interval' seconds")] private int sanityDebuff;
    [SerializeField] [Tooltip("Time interval to apply sanity debuff")] private float sanityDebuffInterval;

    public int GetDifficultyLevel() {
        return (int)difficultyLevel;
    }

    public int GetDifficultyMaxPwrLevel() {
        return difficultyMaxPwrLvl;
    }

    public int GetEnemySpawnInterval() {
        return enemySpawnInterval;
    }

    public int GetDifficultyDayTimeMultiplier() {
        return difficultyDayTimeMultiplier;
    }

    public float GetDifficultyMultiplier() {
        return difficultyMultiplier;
    }

    public int GetSanityDebuff() {
        return sanityDebuff;
    }

    public float GetSanityDebuffInterval() {
        return sanityDebuffInterval;
    }

}
