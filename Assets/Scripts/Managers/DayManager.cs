using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DayManager { 

    private DayBaseState currentState;
    public readonly DayMorningState mornigState = new DayMorningState();
    public readonly GameDayGraphicsData morningDayGraphicsData;
    public readonly DayAfternoonState afternoonState = new DayAfternoonState();
    public readonly GameDayGraphicsData afternoonDayGraphicsData;
    public readonly DayNightState nightState = new DayNightState();
    public readonly GameDayGraphicsData nightDayGraphicsData;

    public readonly GameDifficultyData gameDifficultyData;
    public readonly Light sceneLight;
    public readonly Volume globalVolume;
    public readonly ColorAdjustments colorAdjustments;

    private const float DEFAULT_NEW_GAME_TIME = 0f;
    private const float DEFAULT_START_OF_DAY_TIME = 21600f;
    private const float DEFAULT_MAX_GAME_TIME = 86400f;
    private float gamePlayingTime;
    private float gameTimeMultiplier;

    public DayManager() {
        //Get References
        gameDifficultyData = GameManager.Instance.GetCurrentDifficultyData();
        morningDayGraphicsData = GameManager.Instance.GetMorningGraphicsData();
        afternoonDayGraphicsData = GameManager.Instance.GetAfternoonGraphicsData();
        nightDayGraphicsData = GameManager.Instance.GetNightGraphicsData();
        sceneLight = GameManager.Instance.GetLight();
        globalVolume = GameManager.Instance.GetVolume();
        if(globalVolume.profile.TryGet<ColorAdjustments>(out ColorAdjustments data)) {
            colorAdjustments = data;
        }

        currentState = mornigState;
        currentState.EnterState(this);
    }

    /// <summary>
    /// This method is used in the Morning State to start a new day, hence reset the time
    /// </summary>
    public void StartDay() {
        gamePlayingTime = DEFAULT_NEW_GAME_TIME;
        gameTimeMultiplier = GameManager.Instance.GetCurrentDifficultyData().GetDifficultyDayTimeMultiplier();
    }

    /// <summary>
    /// This method allows to stop day elapsing when the player is on the pause menu or looking through the options
    /// </summary>
    public void PauseDay() {
        //placeholder method
    }

    public void UpdateCurrentState() {
        if(gamePlayingTime < DEFAULT_MAX_GAME_TIME) {
            gamePlayingTime += Time.deltaTime * gameTimeMultiplier;
        }
        currentState.UpdateState(this);
    }

    public void SwitchState(DayBaseState state) {
        currentState = state;
        state.EnterState(this);
    }

    public float GetCurrentGameTime() {
        return gamePlayingTime;
    }

    public string GetCurrentGameTimeInHHMMSS() {
        TimeSpan timeSpan = TimeSpan.FromSeconds(DEFAULT_START_OF_DAY_TIME + gamePlayingTime);
        string timeText = String.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        return timeText;
    }

    public bool IsNight() {
        return currentState == nightState;
    }

}

