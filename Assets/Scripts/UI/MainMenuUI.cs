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
    [SerializeField] private SettingsUI settingsPanel;
    [SerializeField] private GameObject loadingPlaceholder;

    private IMenuUI currentShownSideMenuPanel;

    private void Awake() {
        newGameButton.onClick.AddListener(() => {
            // newGame button clicked
            loadingPlaceholder.SetActive(true);
            LoadingManager.Load(LoadingManager.Scene.LobbyScene);
        });

        loadGameButton.onClick.AddListener(() => {
            // loadGame button clicked
        });

        settingsButton.onClick.AddListener(() => {
            // settings button clicked
            settingsPanel.Show();
            currentShownSideMenuPanel = settingsPanel;
        });

        quitButton.onClick.AddListener(() => {
            // quitGame button clicked
            currentShownSideMenuPanel = alertUI;
            alertUI.SetDisplayInfo("Are you sure you want to quit?");
            alertUI.SetCancelButtonEvent(() => {
                alertUI.Hide();
                currentShownSideMenuPanel = null;
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

    private void Update() {

        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(currentShownSideMenuPanel != null) {
                //Side panel showing -> Hide it and reset ref

                currentShownSideMenuPanel.Hide();
                currentShownSideMenuPanel = null;
            }
        }

    }

}
