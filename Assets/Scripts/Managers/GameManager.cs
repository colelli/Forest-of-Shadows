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

    private void Awake() {
        //We check if there is already a Singleton of GameManager
        if (Instance != null && Instance != this) {
            Destroy(this);
            throw new System.Exception("An Instance of this Singleton already exists");
        } else {
            //There are not instances
            Instance = this;
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

    public bool IsGamePlaying() {
        return state == GameState.GamePlaying;
    }

    public bool IsGamePause() {
        return state == GameState.GamePaused;
    }

}
