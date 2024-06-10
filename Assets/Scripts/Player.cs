using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public event EventHandler OnPlayerDeath;

    private StarterAssetsInputs inputs;
    private MagicalCane magicalCane;
    private Coroutine lightTimerCoroutine;

    private IInteractable interactTarget;

    private bool coroutineRunning;
    private float resetInteractionTimer = 3f;
    private int interactionsWithLight;

    [Header("Sanity System")]
    [SerializeField] private AudioClip _madnessSound;
    [SerializeField] private AudioClip[] _blowTorch;
    private const float maxSanity = 100f;
    private float currentMaxSanity = 100f;
    private float sanity;

    [Header("Stamina System")]
    public readonly float maxStamina = 100f;
    private float stamina;
    private float defaultStaminaRegen = 10f;

    [Header("Health System")]
    public readonly float maxHealth = 100f;
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
        magicalCane = gameObject.GetComponent<MagicalCane>();
        Debug.Log(magicalCane);
        inputs = gameObject.GetComponent<StarterAssetsInputs>();
        inputs.OnInteractAction += Inputs_OnInteractAction;
        
        UpdateFogRingStatus();
    }

    private void Inputs_OnInteractAction(object sender, System.EventArgs e) {

        if (GameManager.Instance.IsGamePaused()) return;

        if (GameManager.Instance.IsNight() && !magicalCane.IsLightOn()) {
            InteractWithLight();
        }else if (interactTarget != null && !interactTarget.IsBusy()) {
            //We are next to a prop that we can pick up
            if (interactTarget.Interact()) {
                interactTarget = null;
                PlayerStatUI.Instance.HideInteractionHintUI();
            }
        }

    }

    private void InteractWithLight() {
        interactionsWithLight++;

        if( interactionsWithLight >= magicalCane.GetNumbersOfInteractionsNeededToTurnOn() ) {
            magicalCane.ToggleLight();
            AudioManager.Instance.StopSoundAtIndex(AudioManager.Indices.SFX);
            if(sanity == 0) {
                currentMaxSanity -= currentMaxSanity * .2f;
                sanity = currentMaxSanity;
            }
        } else {
            if (coroutineRunning) {
                Debug.Log("Coroutine stopped!");
                StopCoroutine(lightTimerCoroutine);
                PlayerStatUI.Instance.HideInteractionHintUI();
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
            currentHealth = 0f;
            Death();
        } else { 
            currentHealth -= dmgAmount;
        }
    }

    public void Heal(float healedAmount) {
        currentHealth = Mathf.Clamp(currentHealth + healedAmount, 0f, 100f);
    }

    public float GetCurrentHealth() {
        return currentHealth;
    }

    /// <summary>
    /// Method to allow entities (and game mangers) to drain player's sanity.<br/>
    /// If the sanity drops below a certain threshold (0 by default), the player will go crazy and turn off his magic light.
    /// </summary>
    public void DrainSanity() {

        if(sanity > 0) {
            sanity = Mathf.Clamp(sanity - GameManager.Instance.GetCurrentDifficultyData().GetSanityDebuff(), 0f, currentMaxSanity);
        } else if(magicalCane.IsLightOn()) {
            //Light is on -> turn it off
            AudioManager.Instance.PlayOneShot(_blowTorch[UnityEngine.Random.Range(0, _blowTorch.Length)]);
            BlowTorch();
            AudioManager.Instance.PlayOneShot(_madnessSound);
        }

    }

    public void RestoreSanity(float restoredAmount) {
        sanity = Mathf.Clamp(sanity + restoredAmount, 0f, 100f);
    }

    public float GetMaxSanity() {
        return maxSanity;
    }

    public float GetCurrentSanity() {
        return sanity;
    }

    public float GetCurrentStamina() {
        return stamina;
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
        PlayerStatUI.Instance.ShowInteractionHintUI();
        magicalCane.ToggleLight();
    }

    private void Death() {
        if(currentHealth <= 0) {
            Debug.Log("Game Over");
            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
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
            PlayerStatUI.Instance.ShowInteractionHintUI();
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.TryGetComponent<IInteractable>(out IInteractable prop)) {
            if(interactTarget == prop) {
                Debug.Log($"[{this.name}] >>> Exited {prop} interaction range");
                interactTarget = null;
                PlayerStatUI.Instance.HideInteractionHintUI();
            }
        }
    }

}
