using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy {

    public void TakeDamage(float dmgAmount);
    public void DealDamage(Player player);
    public bool IsSensibleToLight();

}