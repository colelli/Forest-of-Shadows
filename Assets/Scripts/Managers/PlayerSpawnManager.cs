using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour {

    private static Player inGamePlayer;

    private void Start() {
        inGamePlayer = Instantiate(GameManager.Instance.GetPlayer(), Vector3.zero, Quaternion.identity);
    }

    public static Player GetInGamePlayer() {
        return inGamePlayer;
    }

}
