using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyDoor : MonoBehaviour, IInteractable {
    public bool Interact() {
        if (GameManager.Instance.IsGamePlaying()) {
            // TO-DO: Add checks for score & alert
            LobbyMananger.EnterLobbyAndSaveGame(0);
        } else {
            LobbyMananger.EnterNextLevel();
        }
        return true;
    }
}