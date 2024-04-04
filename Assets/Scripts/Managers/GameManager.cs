using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; private set; }
    public static DayManager DayManager { get; private set; }

    public enum GameState {
        WaitingToStart,
        GamePlaying,
        GamePaused,
        GameOver,
    }

    private GameState state;
    [SerializeField] private Player player;
    [SerializeField] private Transform terrain;
    [SerializeField] private Light worldLight;
    [SerializeField] private bool debugMode;
    [SerializeField] private GameDifficultyData[] gameDifficulties;
    [SerializeField] private int gameDifficultyIndex;
    private string currentTime;

    [Header("Graphics")]
    [SerializeField] private Volume globalVolume;
    [Space(10)]
    [SerializeField] private GameDayGraphicsData morningGraphicsData;
    [SerializeField] private GameDayGraphicsData afternoonGraphicsData;
    [SerializeField] private GameDayGraphicsData nightGraphicsData;

    private void Awake() {
        //We check if there is already a Singleton of GameManager
        if (Instance != null && Instance != this) {
            Destroy(this);
            throw new System.Exception($"[{this.name}] >>> An Instance of this Singleton already exists!");
        } else {
            //There are not instances
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start() {
        ChangeState(GameState.WaitingToStart);
        //ChangeState(GameState.GamePlaying);
    }

    private void Update() {

        switch (state) {

            case GameState.WaitingToStart:
                //player is currently inside the cabin or still hasn't started the day
                break; 
            case GameState.GamePlaying:

                if(DayManager == null) {
                    //We start a new Day
                    Debug.Log($"[{this.name}] >>> Game Started\n");
                    //GetWorldReferences();
                    Instantiate(player, Vector3.zero, Quaternion.identity);
                    DayManager = new DayManager();
                } else {
                    DayManager.UpdateCurrentState();
                    currentTime = DayManager.GetCurrentGameTimeInHHMMSS();
                }

                break;
            case GameState.GamePaused:
                DayManager.PauseDay();
                break;
            case GameState.GameOver:
                break;

        }

    }

    private void GetWorldReferences() {
        //terrain = GameObject.Find("FoS_Terrain").transform;
        worldLight = GameObject.Find("FoS_Directional Light").GetComponent<Light>();
        //globalVolume = GameObject.Find("FoS_GlobalVolume").GetComponent<Volume>();
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

    public bool IsGamePause() {
        return state == GameState.GamePaused;
    }
    public bool IsInDebugMode() {
        return debugMode;
    }

    public Player GetPlayer() {
        return player;
    }

    public Transform GetTerrain() {
        return terrain;
    }

    public Light GetLight() {
        return worldLight;
    }

    public Volume GetVolume() {
        return globalVolume;
    }

    public GameDayGraphicsData GetMorningGraphicsData() {
        return morningGraphicsData;
    }

    public GameDayGraphicsData GetAfternoonGraphicsData() {
        return afternoonGraphicsData;
    }

    public GameDayGraphicsData GetNightGraphicsData() {
        return nightGraphicsData;
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
