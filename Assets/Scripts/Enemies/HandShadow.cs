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

        transform.position += new Vector3(moveAmount.x, 0, moveAmount.z);
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
            if (magicalCane.IsLightOn() && IsSensibleToLight()) {
                //We hit the light source -> the mob takes damage and retreats
                TakeDamage(magicalCane.GetAttackDamage());
                RetreatFromLightSource();
            }  
        }else if(other.TryGetComponent<Player>(out Player player)) {
            //We hit a player
            Debug.Log("Player hit!");
            DealDamage(player);
        }

    }

    public override bool IsSensibleToLight() {
        return true;
    }

}
