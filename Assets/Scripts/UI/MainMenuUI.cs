using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

    public static MainMenuUI Instance { get; private set; }

    [SerializeField] private Button newGameButton;
    [SerializeField] private Button loadGameButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private AlertUI alertUI;
    [SerializeField] private SettingsUI settingsPanel;
    [SerializeField] private DifficultyUI difficultyPanel;
    [SerializeField] private GameObject loadingPlaceholder;
    [SerializeField] private GameObject title;

    private IMenuUI currentShownSideMenuPanel;

    private void Awake() {
        //We check if there is already a Singleton of MainMenuUI
        if (Instance != null && Instance != this) {
            Destroy(this);
            throw new System.Exception($"[{this.name}] >>> An Instance of this Singleton already exists!");
        } else {
            //There are not instances
            Instance = this;
        }

        SetupButtonListeners();
    }

    private void Update() {

        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(currentShownSideMenuPanel != null) {
                //Side panel showing -> Hide it and reset ref

                currentShownSideMenuPanel.Hide();
                ShowTitle();
                currentShownSideMenuPanel = null;
            }
        }

    }

    private void SetupButtonListeners() {
        newGameButton.onClick.AddListener(() => {
            // newGame button clicked
            if (difficultyPanel.ToggleVisibility()) {
                //shown
                currentShownSideMenuPanel = difficultyPanel;
                ToggleTitle();
            } else {
                //hidden
                currentShownSideMenuPanel = null;
                ShowTitle();
            }
        });

        loadGameButton.onClick.AddListener(() => {
            // loadGame button clicked
        });

        settingsButton.onClick.AddListener(() => {
            // settings button clicked
            if (settingsPanel.ToggleVisibility()) {
                //shown
                currentShownSideMenuPanel = settingsPanel;
                ToggleTitle();
            } else {
                //hidden
                currentShownSideMenuPanel = null;
                ShowTitle();
            }
        });

        quitButton.onClick.AddListener(() => {
            // quitGame button clicked
            currentShownSideMenuPanel = alertUI;
            ToggleTitle();
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

    public void ShowLoadingPlaceholder() {
        loadingPlaceholder.SetActive(true);
    }

    private void ShowTitle() {
        title.SetActive(true);
    }

    private void ToggleTitle() {
        title.SetActive(!title.activeSelf);
    }

}
