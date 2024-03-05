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
        //TO-DO move towards target
    }

    private void RetreatFromLightSource() {
        //For the moment we move the mob 10 units back
        transform.position -= Vector3.forward * 10f;
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
