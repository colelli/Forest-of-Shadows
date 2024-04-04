using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindingWheel : MonoBehaviour, IInteractable {

    private const string _ON_INTERACTION_ACTION_PARAM = "OnInteractionAction";

    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _clip;

    private void Start() {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public bool Interact() {
        _audioSource.clip = _clip;
        _animator.SetBool(_ON_INTERACTION_ACTION_PARAM, true);
        _animator.SetBool(_ON_INTERACTION_ACTION_PARAM, false);
        _audioSource.Play();
        return true;
    }

}
