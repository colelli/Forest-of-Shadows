using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyUI : MonoBehaviour, IMenuUI {

    [SerializeField] private Button easy;
    [SerializeField] private Button normal;
    [SerializeField] private Button hard;
    [SerializeField] private Button nightmare;

    public static readonly string _GAMESAVE_FILENAME = "game_data";
    private static GameSaveData _defaultNewGameData = new GameSaveData(1, 1, 0);
    private GameSaveData _currentGameData = null;
    private bool _isVisible = false;

    private void Awake() {
        easy.onClick.AddListener(() => {
            _currentGameData.difficulty = (int)GameDifficultyData.GameDifficultyLevel.Easy;
            StartGame();
        });
        normal.onClick.AddListener(() => {
            _currentGameData.difficulty = (int)GameDifficultyData.GameDifficultyLevel.Normal;
            StartGame();
        });
        hard.onClick.AddListener(() => {
            _currentGameData.difficulty = (int)GameDifficultyData.GameDifficultyLevel.Hard;
            StartGame();
        });
        nightmare.onClick.AddListener(() => {
            _currentGameData.difficulty = (int)GameDifficultyData.GameDifficultyLevel.Nightmare;
            StartGame();
        });
    }
    
    private void Start() {
        TryLoadSavedGameData();
        Hide();
    }
    private void TryLoadSavedGameData() {
        if (SaveManager.TryReadSavedData<GameSaveData>(_GAMESAVE_FILENAME, out GameSaveData gameData)) {
            _currentGameData = gameData;
            _currentGameData.level = 1;
        } else {
            _currentGameData = _defaultNewGameData;
        }
    }

    public void ToggleVisibility() {
        _isVisible = !_isVisible;
        if (_isVisible) { Show(); } else { Hide(); }
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public static GameSaveData GetDefaultSaveData() {
        return _defaultNewGameData;
    }

    private void StartGame() {
        SaveManager.SaveData(_currentGameData, _GAMESAVE_FILENAME);
        MainMenuUI.Instance.ShowLoadingPlaceholder();
        LoadingManager.Load(LoadingManager.Scene.LobbyScene);
    }

}
