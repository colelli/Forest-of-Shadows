using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUp : PropBase {

    [SerializeField] private float healAmount = 15.0f;
    private Player _inGamePlayer;

    private void Start() {
        _inGamePlayer = GameManager.Instance.GetPlayer();
    }

    public override bool Interact() {
        _inGamePlayer.Heal(healAmount);
        AudioManager.Instance.PlayOneShot(_propSO.clips[Random.Range(0, _propSO.clips.Length)]);
        DestroySelf();
        return true;
    }

}
