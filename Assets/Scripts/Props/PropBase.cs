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

    public PropBase SpawnProp(PropSO propSO) {
        Transform propSoTransform = Instantiate(propSO.propPrefab);
        PropBase propBase = propSoTransform.GetComponent<PropBase>();
        return propBase;
    }

    public PropSO GetPropSO() {
        return propSO;
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }

}
