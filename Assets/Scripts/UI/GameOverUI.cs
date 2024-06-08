using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {

    [SerializeField] private Button _returnToLobbyBtn;

    private void Awake() {
        _returnToLobbyBtn.onClick.AddListener(() => {
            Time.timeScale = 1f;
            LobbyMananger.EnterLobbyAfterGameOver();
        });
    }

    private void Start() {
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
        gameObject.SetActive(false);
    }

    private void GameManager_OnGameOver(object sender, System.EventArgs e) {
        gameObject.SetActive(true);
    }

    private void OnDestroy() {
        GameManager.Instance.OnGameOver -= GameManager_OnGameOver;
    }

}