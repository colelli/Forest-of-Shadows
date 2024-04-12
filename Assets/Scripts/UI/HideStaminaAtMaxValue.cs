using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideStaminaAtMaxValue : MonoBehaviour {

    [SerializeField] private GameObject staminaUI;
    private PlayerStatUI playerStatUI;

    private void Start() {
        playerStatUI = GetComponent<PlayerStatUI>();
        playerStatUI.OnHideStamina += PlayerStatUI_OnHideStamina;
        playerStatUI.OnShowStamina += PlayerStatUI_OnShowStamina;
    }

    private void PlayerStatUI_OnShowStamina(object sender, System.EventArgs e) {
        staminaUI.SetActive(true);
    }

    private void PlayerStatUI_OnHideStamina(object sender, System.EventArgs e) {
        staminaUI.SetActive(false);
    }

    public bool IsStaminaBarVisible() {
        return staminaUI.activeInHierarchy;
    }

    private void OnDestroy() {
        playerStatUI.OnHideStamina -= PlayerStatUI_OnHideStamina;
        playerStatUI.OnShowStamina -= PlayerStatUI_OnShowStamina;
    }

}
