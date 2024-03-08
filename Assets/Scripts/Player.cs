using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private StarterAssetsInputs inputs;
    private MagicalCane magicalCane;
    private Coroutine lightTimerCoroutine;

    private IInteractable interactTarget;
    private bool isInteracting;

    private bool coroutineRunning;
    private float resetInteractionTimer = 3f;
    private int interactionsWithLight;

    private const float maxHealth = 100f;
    private float currentHealth;

    private void Awake() {
        isInteracting = false;
        coroutineRunning = false;
        currentHealth = maxHealth;
        interactionsWithLight = 0;
    }

    private void Start() {
        magicalCane = GetComponent<MagicalCane>();
        inputs = GetComponent<StarterAssetsInputs>();
        inputs.OnInteractAction += Inputs_OnInteractAction;
    }

    private void Inputs_OnInteractAction(object sender, System.EventArgs e) {

        if (!magicalCane.IsLightOn()) {
            InteractWithLight();
        }else if (interactTarget != null) {
            //We are next to a prop that we can pick up
            isInteracting = !isInteracting;
            if (interactTarget.Interact() && isInteracting) {
                interactTarget = null;
            }
            isInteracting = !isInteracting;
        }

    }

    private void InteractWithLight() {
        interactionsWithLight++;

        if( interactionsWithLight >= magicalCane.GetNumbersOfInteractionsNeededToTurnOn() ) {
            magicalCane.ToggleLight();
        } else {
            if (coroutineRunning) {
                Debug.Log("Coroutine stopped!");
                StopCoroutine(lightTimerCoroutine);
                coroutineRunning = false;
            }
            lightTimerCoroutine = StartCoroutine(TimerCoroutine(resetInteractionTimer, ResetNumbersOfInteractions));
        }

        coroutineRunning = !coroutineRunning;
    }

    private IEnumerator TimerCoroutine(float timer, Action callback) {
        float counter = timer;
        while (counter > 0) {
            yield return new WaitForSeconds(1);
            counter--;
        }
        callback();
    }

    public void TakeDamage(float dmgAmount) {
        if(currentHealth <= dmgAmount) {
            Death();
        } else { 
            currentHealth -= dmgAmount;
        }
    }

    private void Death() {
        //Player failed, will call GameOver
    }

    private void ResetNumbersOfInteractions() {
        interactionsWithLight = 0;
        coroutineRunning = false;
        Debug.Log("Interaction Resetted!");
    }

    private void OnDestroy() {
        inputs.OnInteractAction -= Inputs_OnInteractAction;
    }
    
    private void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent<IInteractable>(out IInteractable prop)) {
            interactTarget = prop;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.TryGetComponent<IInteractable>(out IInteractable prop)) {
            if(interactTarget == prop) {
                interactTarget = null;
            }
        }
    }

}
