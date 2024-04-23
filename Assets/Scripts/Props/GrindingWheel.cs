using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindingWheel : MonoBehaviour, IInteractable {

    private string _ON_INTERACTION_ACTION_PARAM = "OnInteractionAction";

    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _clip;
    private bool _busy = false;

    public bool Interact() {
        _busy = true;
        _animator.SetBool(_ON_INTERACTION_ACTION_PARAM, true);
        StartCoroutine(BusyInteracting());
        _audioSource.PlayOneShot(_clip[Random.Range(0, _clip.Length)]);
        // this false indicates that the item can be interacted with again
        return false;
    }

    private IEnumerator BusyInteracting() {
        yield return new WaitForSeconds(1.1f);
        _animator.SetBool(_ON_INTERACTION_ACTION_PARAM, false);
        _busy = false;
    }

    public bool IsBusy() {
        return _busy;
    }

}
