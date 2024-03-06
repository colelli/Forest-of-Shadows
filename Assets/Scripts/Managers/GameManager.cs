using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }
    public static DayManager dayManager { get; private set; }

    private enum GameState {
        WaitingToStart,
        GamePlaying,
        GamePaused,
        GameOver,
    }

    private GameState state;
    [SerializeField] private bool debugMode;
    [SerializeField] private GameDifficultyData[] gameDifficulties;
    private int gameDifficultyIndex;

    private void Awake() {
        //We check if there is already a Singleton of GameManager
        if (Instance != null && Instance != this) {
            Destroy(this);
            throw new System.Exception("[{$this.name}] >>> An Instance of this Singleton already exists!");
        } else {
            //There are not instances
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start() {
        if (debugMode) {
            FindObjectOfType<Player>().GetComponent<PlayerDebug>().enabled = true;
        }
    }

    private void Update() {

        switch (state) {

            case GameState.WaitingToStart:
                //player is currently inside the cabin or still hasn't started the day
                break; 
            case GameState.GamePlaying:

                if(dayManager == null) {
                    //We start a new Day
                    dayManager = new DayManager();
                } else {
                    dayManager.UpdateCurrentState();
                }

                break;
            case GameState.GamePaused:
                dayManager.PauseDay();
                break;
            case GameState.GameOver:
                break;

        }

    }

    public GameDifficultyData GetCurrentDifficultyData() {
        return gameDifficulties[gameDifficultyIndex];
    }

    public bool IsGamePlaying() {
        return state == GameState.GamePlaying;
    }

    public bool IsGamePause() {
        return state == GameState.GamePaused;
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
    [SerializeField] [Min(1)] private int difficultyMaxPwrLvl;
    [SerializeField] [Min(15)] private int enemySpawnInterval;
    [SerializeField] [Min(1)] private int difficultyDayTimeMultiplier;

    public int GetDifficultyMaxPwrLevel() {
        return difficultyMaxPwrLvl;
    }

    public int GetEnemySpawnInterval() {
        return enemySpawnInterval;
    }

}
