using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyDoor : MonoBehaviour, IInteractable {

    [SerializeField] private AudioClip _clip;

    public bool Interact() {
        if (GameManager.Instance.IsGamePlaying()) {
            // TO-DO: Add checks for score & alert
            LobbyMananger.EnterLobbyAndSaveGame(0);
        } else {
            LobbyMananger.EnterNextLevel();
        }
        AudioManager.Instance.PlayOneShot(_clip);
        return true;
    }

    public bool IsBusy() {
        // No need to be busy since it will load a new scene on interaction
        return false;
    }

}