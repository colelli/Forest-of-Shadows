using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanityPowerUp : PropBase {

    [SerializeField] private float _sanityRestoreAmount = 30.0f;
    private Player _inGamePlayer;

    private void Start() {
        _inGamePlayer = GameManager.Instance.GetPlayer();
    }

    public override bool Interact() {
        _inGamePlayer.RestoreSanity(_sanityRestoreAmount);
        AudioManager.Instance.PlayOneShot(_propSO.clips[Random.Range(0, _propSO.clips.Length)]);
        DestroySelf();
        return true;
    }

}
