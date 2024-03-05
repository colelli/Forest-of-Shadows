using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private const float maxHealth = 100f;
    private float currentHealth;

    private void Awake() {
        currentHealth = maxHealth;
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

}
