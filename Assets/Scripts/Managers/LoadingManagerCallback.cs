using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingManagerCallback : MonoBehaviour {

    private bool isFirstUpdate = true;

    private void Update() {
        if (isFirstUpdate) {
            isFirstUpdate = false;

            // We only load the next scene when the "loading" scene
            // is visible for at least a frame
            LoadingManager.LoadingManagerCallback();
        }
    }

}
