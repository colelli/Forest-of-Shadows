using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMananger : MonoBehaviour {

    [SerializeField] private Player playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;

    public LobbyMananger Instance { get; private set; }
    private static GameSaveData _currentSave;
    private const string _DEFAULT_SAVED_GAME_FILENAME = "game_data";

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
    }

    private void Start() {
        if (SaveManager.TryReadSavedData<GameSaveData>(_DEFAULT_SAVED_GAME_FILENAME, out GameSaveData savedGame)) {
            _currentSave = savedGame;
        } else {
            _currentSave = DifficultyUI.GetDefaultSaveData();
        }

        // Spawn player in Lobby
        Player player = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
        GameManager.Instance.SetPlayerOnSpawn(player);
    }

    public static void EnterNextLevel() {
        // Load to scene
        LoadingManager.Load(LoadingManager.Scene.GameScene);
    }

    public static void EnterLobbyAndSaveGame(int score) {
        // Save new progress
        SaveManager.SaveData<GameSaveData>(new GameSaveData(_currentSave.difficulty, _currentSave.level + 1, _currentSave.score + score), _DEFAULT_SAVED_GAME_FILENAME);
        LoadingManager.Load(LoadingManager.Scene.LobbyScene);
    }

    public static int GetCurrentLevel() {
        return _currentSave.level;
    }

    public static int GetCurrentScore() {
        return _currentSave.score;
    }

}