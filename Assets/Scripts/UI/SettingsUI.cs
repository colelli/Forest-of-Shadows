using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour, IMenuUI {

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI forward;
    [SerializeField] private TextMeshProUGUI left;
    [SerializeField] private TextMeshProUGUI back;
    [SerializeField] private TextMeshProUGUI right;
    [SerializeField] private TextMeshProUGUI interact;
    [Space(10)]
    [SerializeField] private Slider masterVolumeSlider;
    [Space(10)]
    [SerializeField] private Toggle vignetteToggle;

    private const string _SETTINGS_FILENAME = "settings";
    private SettingsData _defaultSettings = new SettingsData(true,"W","A","S","D","E",50.0f);
    private SettingsData _savedSettings = null;

    private void Start() {
        TryLoadSavedSettings();
        Hide();
    }

    private void TryLoadSavedSettings() {
        if(SaveManager.TryReadSavedData<SettingsData>(_SETTINGS_FILENAME, out SettingsData settings)) {
            _savedSettings = settings;
        } else {
            _savedSettings = _defaultSettings;
        }
        //Modify keybindings UI
        forward.text = _savedSettings.moveUpKey;
        left.text = _savedSettings.moveLeftKey;
        back.text = _savedSettings.moveDownKey;
        right.text = _savedSettings.moveRightKey;
        interact.text = _savedSettings.interactKey;

        //Modify slider UI
        masterVolumeSlider.value = _savedSettings.masterVolume;

        //Modify checkbox UI
        vignetteToggle.isOn = _savedSettings.vignittePP;
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        SaveManager.SaveData(GetCurrentUserSettings(), _SETTINGS_FILENAME);
        gameObject.SetActive(false);
    }

    private SettingsData GetCurrentUserSettings() {
        if (_savedSettings == null) {
            _savedSettings = _defaultSettings;
        } else {
            // get settings from UI

        }
        return _savedSettings;
    }

}