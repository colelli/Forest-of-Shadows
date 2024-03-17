using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PropBase : MonoBehaviour, IInteractable {

    [SerializeField] private PropSO propSO;
    private SphereCollider propCollider;

    private void Start() {
        propCollider = GetComponent<SphereCollider>();
        propCollider.isTrigger = true;
        transform.localScale *= 2f;
    }

    public PropSO GetPropSO() {
        return propSO;
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }

    public bool Interact() {
        //Player interacted -> We deliver the prop and notify the result
        if (DeliveryManager.Instance.DeliverProp(this)) {
            return true;
        }
        return false;
    }

}
