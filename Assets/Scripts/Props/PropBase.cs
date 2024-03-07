using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PropBase : MonoBehaviour {

    [SerializeField] private PropSO propSO;
    private SphereCollider propCollider;

    private void Start() {
        propCollider = GetComponent<SphereCollider>();
        propCollider.isTrigger = true;
    }

    public PropSO GetPropSO() {
        return propSO;
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }

}
