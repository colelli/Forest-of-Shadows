using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PropBase : MonoBehaviour, IInteractable {

    [SerializeField] private PropSO propSO;
    [SerializeField] [Min(1f)] private float localScale;
    private BoxCollider propCollider;

    private void Start() {
        propCollider = GetComponent<BoxCollider>();
        propCollider.isTrigger = true;
        transform.localScale *= localScale;
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
