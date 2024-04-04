using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class LobbyMananger : MonoBehaviour {

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;

    public LobbyMananger Instance { get; private set; }
    private GameSaveData _newGameDefaultSave;
    private static GameSaveData _currentSave;
    private const string _DEFAULT_SAVED_GAME_FILENAME = "savedgame";

    private void Awake() {
        //We check if there is already a Singleton of LobbyManager
        if (Instance != null && Instance != this) {
            Destroy(this);
            throw new System.Exception($"[{this.name}] >>> An Instance of this Singleton already exists!");
        } else {
            //There are not instances
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        _newGameDefaultSave = new GameSaveData(0, 0);
    }

    private void Start() {
        if (SaveManager.TryReadSavedData<GameSaveData>(_DEFAULT_SAVED_GAME_FILENAME, out GameSaveData savedGame)) {
            _currentSave = savedGame;
        } else {
            _currentSave = _newGameDefaultSave;
        }

        // Spawn player in Lobby
        GameObject player = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity) as GameObject;
    }

    public static void EnterNextLevel() {
        // Load to scene
        LoadingManager.Load(LoadingManager.Scene.GameScene);
    }

    public static void EnterLobbyAndSaveGame(int score) {
        // Save new progress
        SaveManager.SaveData<GameSaveData>(new GameSaveData(_currentSave.level + 1, _currentSave.score + score), _DEFAULT_SAVED_GAME_FILENAME);
        LoadingManager.Load(LoadingManager.Scene.LobbyScene);
    }

    public static int GetCurrentLevel() {
        return _currentSave.level;
    }

    public static int GetCurrentScore() {
        return _currentSave.score;
    }

}