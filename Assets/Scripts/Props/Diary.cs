using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diary : MonoBehaviour, IInteractable {

    public static event EventHandler OnGamePausedUI;
    public static event EventHandler OnGameUnpausedUI;

    [SerializeField] private AudioClip[] _sound;
    [SerializeField] private GameObject _bookUI;
    private AudioSource _audioSource;
    private bool _isBusy = false;

    private void Start() {
        StarterAssetsInputs.OnEscapePressed += StarterAssetsInputs_OnEscapePressed;
        _audioSource = GetComponent<AudioSource>();
        _bookUI.SetActive(false);
    }

    private void StarterAssetsInputs_OnEscapePressed(object sender, EventArgs e) {
        if (IsBusy()) {
            Hide();
        }
    }

    public bool Interact() {
        _audioSource.PlayOneShot(_sound[UnityEngine.Random.Range(0, _sound.Length)]);
        Show();
        return false;
    }

    public bool IsBusy() {
        return _isBusy;
    }

    private void SetBusy(bool value) {
        _isBusy = value;
    }

    private void Show() {
        _bookUI.SetActive(true);
        SetBusy(true);
        OnGamePausedUI?.Invoke(this, EventArgs.Empty);
        PlayerStatUI.Instance.HideInteractionHintUI();
        Time.timeScale = 0f;
    }

    private void Hide() {
        OnGameUnpausedUI?.Invoke(this, EventArgs.Empty);
        SetBusy(false);
        _bookUI.SetActive(false);
        PlayerStatUI.Instance.ShowInteractionHintUI();
        Time.timeScale = 1f;
    }

}
