using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerStatUI : MonoBehaviour {

    public event EventHandler OnHideStamina;
    public event EventHandler OnShowStamina;

    [SerializeField] private Image healthBar;
    [SerializeField] private Image staminaBar;
    private float staminaEffectMin = 0.1f;
    private float staminaEffectMax = 0.9f;

    private void Start() {
        healthBar.fillAmount = 1f;
        staminaBar.fillAmount = 1f;
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

}
