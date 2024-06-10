using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerStatUI : MonoBehaviour {

    public static PlayerStatUI Instance {get; private set;}

    public event EventHandler OnHideStamina;
    public event EventHandler OnShowStamina;

    [SerializeField] private Image healthBar;
    [SerializeField] private Image staminaBar;
    [SerializeField] private GameObject interactionHintUI;
    [SerializeField] private TextMeshProUGUI interactionText;
    private float staminaEffectMin = 0.1f;
    private float staminaEffectMax = 0.9f;

    private void Awake() {
        //We check if there is already a Singleton of AudioManager
        if (Instance != null && Instance != this) {
            Destroy(this);
            throw new System.Exception($"[{this.name}] >>> An Instance of this Singleton already exists!");
        } else {
            //There are not instances
            Instance = this;
        }
    }

    private void Start() {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;

        Show();

        healthBar.fillAmount = 1f;
        staminaBar.fillAmount = 1f;
    }

    private void GameManager_OnGameUnpaused(object sender, EventArgs e) {
        Show();
    }

    private void GameManager_OnGamePaused(object sender, EventArgs e) {
        Hide();
    }

    private void LateUpdate() {
        UpdateUI();
    }

    private void UpdateUI() {
        Player player = GameManager.Instance.GetPlayer();
        healthBar.fillAmount = player.GetCurrentHealth() / player.maxHealth;
        float staminaValue = player.GetCurrentStamina() / player.maxStamina;
        staminaBar.fillAmount = staminaValue;

        if(staminaValue == 1) {
            if (GetComponent<HideStaminaAtMaxValue>().IsStaminaBarVisible()) {
                OnHideStamina?.Invoke(this, EventArgs.Empty);
            }
        }else if (!GetComponent<HideStaminaAtMaxValue>().IsStaminaBarVisible()) {
            OnShowStamina?.Invoke(this, EventArgs.Empty);
        }

        if (!GameManager.Instance.IsGamePlaying() || DayManager.Instance == null) return;

        Volume globalVolume = DayManager.Instance.GetGlobalVolume();
        if(globalVolume.profile.TryGet<Vignette>(out Vignette vignette)) {
            float value = player.GetCurrentSanity() / player.GetMaxSanity();
            float clampedValue = Mathf.Clamp(Mathf.Abs(value - 1f), staminaEffectMin, staminaEffectMax);
            vignette.intensity.SetValue(new UnityEngine.Rendering.ClampedFloatParameter(Mathf.Lerp(vignette.intensity.value, clampedValue, Time.deltaTime), staminaEffectMin, staminaEffectMax));
        }
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void OnDestroy() {
        GameManager.Instance.OnGamePaused -= GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused -= GameManager_OnGameUnpaused;
    }

    public void ShowInteractionHintUI() {
        var gamepad = Gamepad.current;
        switch (gamepad) {
            case null:
                //player plays with keyboard
                interactionText.color = Color.black;
                interactionText.SetText("E");
                break;
            case DualShockGamepad:
                //we have a ps controller connected
                interactionText.color = Color.red;
                interactionText.SetText("O");
                break;
            case XInputController:
                //we have an xbox controller connected
                interactionText.color = Color.red;
                interactionText.SetText("B");
                break;
        }
        interactionHintUI.SetActive(true);
    }

    public void HideInteractionHintUI() {
        interactionHintUI.SetActive(false);
    }

}
