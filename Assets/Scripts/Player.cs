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

    [Header("Sanity System")]
    private float maxSanity = 100f;
    private float sanity;

    [Header("Stamina System")]
    private float maxStamina = 100f;
    private float stamina;
    private float defaultStaminaRegen = 10f;

    [Header("Health System")]
    private const float maxHealth = 100f;
    private float currentHealth;

    [SerializeField] private GameObject fogRing;

    private void Awake() {
        coroutineRunning = false;
        sanity = maxSanity;
        stamina = maxStamina;
        currentHealth = maxHealth;
        interactionsWithLight = 0;
    }

    private void Start() {
        magicalCane = GetComponent<MagicalCane>();
        inputs = GetComponent<StarterAssetsInputs>();
        inputs.OnInteractAction += Inputs_OnInteractAction;
        
        UpdateFogRingStatus();
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
        } else if(magicalCane.IsLightOn()) {
            //Light is on -> turn it off
            BlowTorch();
        }

    }

    public void DecreaseStamina(float amount) {
        stamina = Mathf.Clamp(stamina - amount, 0.0f, maxStamina);
    }

    public void IncreaseStamina() {
        stamina = Mathf.Clamp(stamina + (defaultStaminaRegen * Time.deltaTime), 0.0f, maxStamina);
    }

    public bool CanRun() {
        return stamina > 0.0f;
    }

    public bool HasEnoughStamina(float staminaNeeded) {
        return stamina >= staminaNeeded;
    }

    private void UpdateFogRingStatus() {
        fogRing.SetActive(GameManager.Instance.IsGamePlaying());
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
            Debug.Log($"[{this.name}] >>> Entered {prop} interaction range");
            interactTarget = prop;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.TryGetComponent<IInteractable>(out IInteractable prop)) {
            if(interactTarget == prop) {
                Debug.Log($"[{this.name}] >>> Exited {prop} interaction range");
                interactTarget = null;
            }
        }
    }

}
