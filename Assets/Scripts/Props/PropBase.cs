using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PropBase : MonoBehaviour, IInteractable {

    [SerializeField] private PropSO _propSO;
    [SerializeField] [Min(1f)] private float _localScale;
    private BoxCollider _propCollider;

    private void Start() {
        _propCollider = GetComponent<BoxCollider>();
        _propCollider.isTrigger = true;
        transform.localScale *= _localScale;
    }

    public PropSO GetPropSO() {
        return _propSO;
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }

    public bool Interact() {
        //Player interacted -> We deliver the prop and notify the result
        if (DeliveryManager.Instance.DeliverProp(this)) {
            AudioManager.Instance.PlayOneShot(_propSO.clips[Random.Range(0, _propSO.clips.Length)]);
            DestroySelf();
            return true;
        }
        return false;
    }

    public bool IsBusy() {
        // No need to be busy since it will be picked up on interaction
        return false;
    }

}
