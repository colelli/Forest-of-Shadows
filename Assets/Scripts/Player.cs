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

    private bool coroutineRunning;
    private float resetInteractionTimer = 3f;
    private int interactionsWithLight;

    private float maxSanity = 100f;
    private float sanity;
    private const float maxHealth = 100f;
    private float currentHealth;

    private void Awake() {
        coroutineRunning = false;
        sanity = maxSanity;
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
            if (interactTarget.Interact()) {
                interactTarget = null;
            }
        }

    }

    private void InteractWithLight() {
        interactionsWithLight++;

        if( interactionsWithLight >= magicalCane.GetNumbersOfInteractionsNeededToTurnOn() ) {
            magicalCane.ToggleLight();
            if(sanity == 0) {
                maxSanity -= maxSanity * .2f;
                sanity = maxSanity;
            }
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

    /// <summary>
    /// Method to allow entities to hit and damage the player.
    /// </summary>
    /// <param name="dmgAmount">Amount of damage taken</param>
    public void TakeDamage(float dmgAmount) {
        if(currentHealth <= dmgAmount) {
            Death();
        } else { 
            currentHealth -= dmgAmount;
        }
    }

    /// <summary>
    /// Method to allow entities (and game mangers) to drain player's sanity.<br/>
    /// If the sanity drops below a certain threshold (0 by default), the player will go crazy and turn off his magic light.
    /// </summary>
    public void DrainSanity() {

        if(sanity > 0) {
            sanity = Mathf.Clamp(sanity - GameManager.Instance.GetCurrentDifficultyData().GetSanityDebuff(), 0f, maxSanity);
        } else { 
            BlowTorch();
        }

    }

    private void BlowTorch() {
        magicalCane.ToggleLight();
    }

    private void Death() {
        if(currentHealth <= 0) {
            Debug.Log("Game Over");
        }
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
