using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class MagicalCane : MonoBehaviour {

    [Header("Light Source Configs")]
    [SerializeField] private bool autoUpdate;
    [SerializeField] [Min(1.5f)] [Range(1.5f, 5f)] private float lightSourceRadius;
    [SerializeField] [Tooltip("Toggles IsTrigger in Collider options")] private bool isColliderTrigger = true;
    private int interactionsNeededToTurnOn = 5;
    [SerializeField] private float baseAttackDamage = 10f;
    [Header("Light & Visuals")]
    [SerializeField] private bool isLightOn;
    [SerializeField] private Light lightSource;
    [SerializeField] private Transform lightVisual;
    private float attackDamage;

    private SphereCollider lightSourceCollider;

    private void Awake() {
        DayManager.OnNightStarted += DayManager_OnNightStarted;

        lightSourceCollider = GetComponent<SphereCollider>();
        attackDamage = baseAttackDamage;

        isLightOn = false;
        UpdateLightStatus();
    }

    private void DayManager_OnNightStarted(object sender, System.EventArgs e) {
        ToggleLight();
    }

    private void Start() {
        SetupLightSourceCollider();
    }

    public void SetupLightSourceCollider() {
        if(lightSourceCollider == null) lightSourceCollider = GetComponent<SphereCollider>();

        lightSourceCollider.radius = lightSourceRadius; //TO-DO: Add powerup 
        lightSourceCollider.isTrigger = isColliderTrigger;
    }

    public int GetNumbersOfInteractionsNeededToTurnOn() {
        return interactionsNeededToTurnOn;
    }

    public bool IsLightOn() {
        return isLightOn;
    }
    public void ToggleLight() {
        isLightOn = !isLightOn;
        UpdateLightStatus();
        Debug.Log($"Light Status: {isLightOn}");
    }

    private void UpdateLightStatus() {
        lightSourceCollider.enabled = isLightOn;
        lightSource.enabled = isLightOn;
        lightVisual.gameObject.SetActive(isLightOn);
    }

    public float GetAttackDamage() {
        return attackDamage;
    }

    public bool CanAutoUpdate() {
        return autoUpdate;
    }

    private void OnDestroy() {
        DayManager.OnNightStarted -= DayManager_OnNightStarted;
    }

}
