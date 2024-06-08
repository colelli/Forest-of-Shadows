using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour {

    [SerializeField] private Transform container;
    [SerializeField] private Transform propTemplateUI;

    private void Awake() {
        propTemplateUI.gameObject.SetActive(false);
    }

    private void Start() {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;

        DeliveryManager.Instance.OnPropsListGenerated += DeliveryManager_OnPropsListGenerated;
        DeliveryManager.Instance.OnPropsDelivered += DeliveryManager_OnPropsDelivered;

        UpdateVisuals();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void GameManager_OnGameOver(object sender, System.EventArgs e) {
        Hide();
    }

    private void GameManager_OnGamePaused(object sender, System.EventArgs e) {
        Hide();
    }

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e) {
        Show();
    }

    private void DeliveryManager_OnPropsDelivered(object sender, System.EventArgs e) {
        UpdateVisuals();
    }

    private void DeliveryManager_OnPropsListGenerated(object sender, System.EventArgs e) {
        UpdateVisuals();
    }

    private void UpdateVisuals() {

        // Clean-up logic
        foreach (Transform child in container) {
            if (child == propTemplateUI) continue;
            Destroy(child.gameObject);
        }

        // Instantiate deliverables UI
        foreach (KeyValuePair<PropSO, int> entry in DeliveryManager.Instance.GetDeliverablesList()) {
            Transform propTrasform = Instantiate(propTemplateUI, container);
            propTrasform.GetComponent<PropTemplateUI>().UpdateContent(entry);
            propTrasform.gameObject.SetActive(true);
        }

    }

    private void OnDestroy() {
        DeliveryManager.Instance.OnPropsListGenerated -= DeliveryManager_OnPropsListGenerated;
        DeliveryManager.Instance.OnPropsDelivered -= DeliveryManager_OnPropsDelivered;

        GameManager.Instance.OnGamePaused -= GameManager_OnGamePaused;
        GameManager.Instance.OnGameUnpaused -= GameManager_OnGameUnpaused;
        GameManager.Instance.OnGameOver -= GameManager_OnGameOver;
    }

}
