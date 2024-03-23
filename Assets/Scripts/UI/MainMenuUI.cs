using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private AlertUI alertUI;

    private void Awake() {
        newGameButton.onClick.AddListener(() => {
            // newGame button clicked
            LoadingManager.Load(LoadingManager.Scene.LobbyScene);
        });

        loadGameButton.onClick.AddListener(() => {
            // loadGame button clicked
        });

        settingsButton.onClick.AddListener(() => {
            // settings button clicked
        });

        quitButton.onClick.AddListener(() => {
            // quitGame button clicked
            alertUI.SetDisplayInfo("Are you sure you want to quit?");
            alertUI.SetCancelButtonEvent(() => {
                alertUI.Hide();
            });
            alertUI.SetConfirmButtonEvent(() => {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            });
            alertUI.Show();
        });
    }

}
