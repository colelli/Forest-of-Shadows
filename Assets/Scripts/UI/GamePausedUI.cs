using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePausedUI : MonoBehaviour {

    [Header("Buttons")]
    [SerializeField] private Button settings;
    [SerializeField] private Button quitGame;
    [Header("Screens")]
    [SerializeField] private SettingsUI settingsUI;
    [SerializeField] private AlertUI alertUI;

    private IMenuUI currentShownSideMenuPanel;

    private void Start() {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;

        Hide();
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            HideSidePanel();
        }

    }

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e) {
        Cursor.visible = false;
        HideSidePanel();
        Hide();
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e) {
        Cursor.visible = true;
        settingsUI.ResetVisibility();
        alertUI.ResetVisibility();
        Show();
    }

    private void Hide() {
        ResetListeners();
        gameObject.SetActive(false);
    }

    private void Show() {
        SetupButtonListeners();
        gameObject.SetActive(true);
    }

    private void HideSidePanel() {
        if (currentShownSideMenuPanel != null) {
            //Side panel showing -> Hide it and reset ref

            currentShownSideMenuPanel.Hide();
            currentShownSideMenuPanel = null;
        }
    }

    private void SetupButtonListeners() {
        Debug.Log("Setting listeners");
        settings.onClick.AddListener(() => {
            // settings button clicked
            settingsUI.ToggleVisibility();
            currentShownSideMenuPanel = settingsUI;
        });

        quitGame.onClick.AddListener(() => {
            // quitGame button clicked
            currentShownSideMenuPanel = alertUI;
            alertUI.SetDisplayInfo("You don't really want to quit, do you?");
            alertUI.SetCancelButtonEvent(() => {
                alertUI.Hide();
                alertUI.ResetVisibility();
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

    private void ResetListeners() {
        settings.onClick.RemoveAllListeners();
        quitGame.onClick.RemoveAllListeners();
    }

    private void OnDestroy() {
        GameManager.Instance.OnGamePaused -= GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused -= GameManager_OnGameUnpaused;
    }


}
