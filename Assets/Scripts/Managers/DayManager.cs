using UnityEngine;

public class DayManager { 

    private DayBaseState currentState;
    public readonly DayMorningState mornigState = new DayMorningState();
    public readonly DayAfternoonState afternoonState = new DayAfternoonState();
    public readonly DayNightState nightState = new DayNightState();

    private const float DEFAULT_NEW_GAME_TIME = 0f;
    private float gamePlayingTime;
    private float gameTimeMultiplier = 10f;

    public DayManager() {
        currentState = mornigState;
        currentState.EnterState(this);
    }

    /// <summary>
    /// This method is used in the Morning State to start a new day, hence reset the time
    /// </summary>
    public void StartDay() {
        gamePlayingTime = DEFAULT_NEW_GAME_TIME;
    }

    /// <summary>
    /// This method allows to stop day elapsing when the player is on the pause menu or looking through the options
    /// </summary>
    public void PauseDay() {
        //placeholder method
    }

    public void UpdateCurrentState() {
        gamePlayingTime += Time.deltaTime * gameTimeMultiplier;
        currentState.UpdateState(this);
    }

    public void SwitchState(DayBaseState state) {
        currentState = state;
        state.EnterState(this);
    }

    public float GetCurrentGameTime() {
        return gamePlayingTime;
    }

}

