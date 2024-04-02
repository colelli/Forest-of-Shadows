using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlertUI : MonoBehaviour, IMenuUI {

    [SerializeField] private Image bgPanel;
    [SerializeField] private TextMeshProUGUI displayedInfo;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    private void Awake() {
        gameObject.SetActive(false);
    }

    public void SetConfirmButtonEvent(Action action) {
        confirmButton.onClick.AddListener(() => {
            action();
            confirmButton.onClick.RemoveAllListeners();
        });
    }

    public void SetCancelButtonEvent(Action action) {
        cancelButton.onClick.AddListener(() => {
            action();
            cancelButton.onClick.RemoveAllListeners();
        });
    }

    public void Show() {
        gameObject.SetActive (true);
    }

    public void Hide() {
        gameObject.SetActive (false);
    }

    public void SetDisplayInfo(string text) { 
        displayedInfo.text = text;
    }

}
