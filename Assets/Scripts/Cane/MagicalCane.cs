using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MagicalCane : MonoBehaviour {

    [Header("Light Source Transform")]
    [SerializeField] private Transform lightSource;

    [Header("Light Source Configs")]
    [SerializeField] [Min(1.5f)] [Range(1.5f, 5f)] private float lightSourceRadius;
    [SerializeField] [Tooltip("Toggles IsTrigger in Collider options")] private bool isColliderTrigger = true;
    [SerializeField] private bool isLightOn;
    [SerializeField] private bool canPushShadowsAway = true;
    [SerializeField] private LayerMask shadowMask;

    private SphereCollider lightSourceCollider;

    private void Awake() {
        lightSourceCollider = lightSource.GetComponent<SphereCollider>();
    }

    private void Start() {
        SetupLightSourceCollider();
    }

    private void SetupLightSourceCollider() {
        lightSourceCollider.radius = lightSourceRadius; //TO-DO: Add powerup 
        lightSourceCollider.isTrigger = isColliderTrigger;
    }

    public bool IsLightOn() {
        return isLightOn;
    }
    public void ToggleLight() {
        isLightOn = !isLightOn;
    }

    public bool CanPushShadowAway() {
        return canPushShadowsAway;
    }

    public void ToggleShadowPushAbility() {
        canPushShadowsAway = !canPushShadowsAway;
    }

}
