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
    [SerializeField] private bool isLightOn;
    [SerializeField] private float baseAttackDamage = 10f;
    private float attackDamage;

    private SphereCollider lightSourceCollider;

    private void Awake() {
        lightSourceCollider = GetComponent<SphereCollider>();
        attackDamage = baseAttackDamage;
    }

    private void Start() {
        SetupLightSourceCollider();
    }

    public void SetupLightSourceCollider() {
        if(lightSourceCollider == null) lightSourceCollider = GetComponent<SphereCollider>();

        lightSourceCollider.radius = lightSourceRadius; //TO-DO: Add powerup 
        lightSourceCollider.isTrigger = isColliderTrigger;
    }

    public bool IsLightOn() {
        return isLightOn;
    }
    public void ToggleLight() {
        isLightOn = !isLightOn;
    }

    public float GetAttackDamage() {
        return attackDamage;
    }

    public bool CanAutoUpdate() {
        return autoUpdate;
    }

}
