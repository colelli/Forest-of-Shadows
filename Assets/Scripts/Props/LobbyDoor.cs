using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyDoor : MonoBehaviour, IInteractable {

    [SerializeField] private AudioClip _clip;
    [SerializeField] private AudioClip _errorClip;

    public bool Interact() {
        AudioClip clipToPlay = null;
        if (GameManager.Instance.IsGamePlaying()) {
            if(DeliveryManager.Instance.CanReturnToLobby()) {
                GameManager.Instance.ChangeState(GameManager.GameState.WaitingToStart);
                LobbyMananger.EnterLobbyAndSaveGame(0);
                clipToPlay = _clip;
            } else {
                // TO-DO: Show alert
                clipToPlay = _errorClip;
            }
        } else {
            LobbyMananger.EnterNextLevel();
            clipToPlay = _clip;
        }
        AudioManager.Instance.PlayOneShot(_clip);
        return true;
    }

    public bool IsBusy() {
        // No need to be busy since it will load a new scene on interaction
        return false;
    }

}