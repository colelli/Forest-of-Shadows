using System;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DayManager : MonoBehaviour {

    public static event EventHandler OnNightStarted;

    public static DayManager Instance { get; private set; }

    [Header("Graphics References")]
    [SerializeField] private Light globalLight;
    [SerializeField] private Volume globalVolume;
    [Space(10)]
    [SerializeField] private GameDayGraphicsData morningGraphicsData;
    [SerializeField] private GameDayGraphicsData afternoonGraphicsData;
    [SerializeField] private GameDayGraphicsData nightGraphicsData;

    private DayBaseState currentState;
    public readonly DayMorningState mornigState = new DayMorningState();
    public readonly DayAfternoonState afternoonState = new DayAfternoonState();
    public readonly DayNightState nightState = new DayNightState();

    public GameDifficultyData gameDifficultyData;
    private ColorAdjustments colorAdjustments;

    private const float DEFAULT_NEW_GAME_TIME = 0f;
    private const float DEFAULT_START_OF_DAY_TIME = 21600f;
    private const float DEFAULT_MAX_GAME_TIME = 86400f;
    private float gamePlayingTime;
    private float gameTimeMultiplier;

    private void Awake() {
        //We check if there is already a Singleton of DayManager
        if (Instance != null && Instance != this) {
            Destroy(this);
            throw new System.Exception($"[{this.name}] >>> An Instance of this Singleton already exists!");
        } else {
            //There are not instances
            Instance = this;
        }
    }

    private void Start() {
        //Get References
        gameDifficultyData = GameManager.Instance.GetCurrentDifficultyData();
        if (globalVolume.profile.TryGet<ColorAdjustments>(out ColorAdjustments data)) {
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

        if(state == nightState) {
            // Night started -> Notify subs
            OnNightStarted?.Invoke(this, EventArgs.Empty);
        }
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

    public Light GetGlobalLight() {
        return globalLight;
    }

    public Volume GetGlobalVolume() {
        return globalVolume;
    }

    public ColorAdjustments GetColorAdjustments() { return colorAdjustments; }
    public GameDayGraphicsData GetMorningGraphicsData() { return morningGraphicsData; }
    public GameDayGraphicsData GetAfternoonGraphicsData() { return afternoonGraphicsData; }
    public GameDayGraphicsData GetNightGraphicsData() { return nightGraphicsData; }
    public float GetGameTimeMultiplier() { return gameTimeMultiplier; }

}

[System.Serializable]
public struct GameDayGraphicsData {
    [SerializeField] private Color colour;
    [SerializeField][Range(1500, 20000)] private float lightTemperature;
    [SerializeField][Min(1)] private float lightIntensity;
    [SerializeField] private Color volumeTint;
    [SerializeField] private float volumeExposure;

    public GameDayGraphicsData(Color colour, float lightTemperature, float lightIntensity, Color volumeTint, float volumeExposure = 0f) {
        this.colour = colour;
        this.lightTemperature = lightTemperature;
        this.lightIntensity = lightIntensity;
        this.volumeTint = volumeTint;
        this.volumeExposure = volumeExposure;
    }

    public Color GetLightColour() {
        return colour;
    }

    public float GetLightTemperature() {
        return lightTemperature;
    }

    public float GetLightIntensity() {
        return lightIntensity;
    }

    public Color GetVolumeTint() {
        return volumeTint;
    }

    public float GetVolumeExposure() {
        return volumeExposure;
    }
}
