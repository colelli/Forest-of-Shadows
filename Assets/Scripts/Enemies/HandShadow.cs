using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandShadow : Enemy {

    protected new void Awake() {
        base.Awake();
        mobCollider = GetComponent<BoxCollider>();
        mobCollider.isTrigger = true;
    }

    protected override void Move() {
        //Move towards target
        Vector3 displacementFromTarget = target.position -  transform.position;
        Vector3 moveDirection = displacementFromTarget.normalized;
        Vector3 velocity = moveDirection * enemyType.enemyMaxMovementSpeed;
        Vector3 moveAmount = velocity * Time.deltaTime;

        transform.position += moveAmount;
    }

    private void RetreatFromLightSource() {
        //For the moment we move the mob 10 units back
        Vector3 moveDirection = (target.position - transform.position).normalized;
        Vector3 moveAmount = moveDirection * -10f;
        transform.Translate(moveAmount);
        StunSelf();
    }

    private void OnTriggerEnter(Collider other) {

        if(other.TryGetComponent<MagicalCane>(out MagicalCane magicalCane)) {
            //We hit the light source -> the mob takes damage and retreats
            TakeDamage(10f);
            RetreatFromLightSource();
        }

        if(other.TryGetComponent<Player>(out Player player)) {
            //We hit a player
            DealDamage(player);
        }

    }

    public override bool IsSensibleToLight() {
        return true;
    }

}
