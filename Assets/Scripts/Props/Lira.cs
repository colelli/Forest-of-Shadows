using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lira : MonoBehaviour, IInteractable {

    [SerializeField] private AudioClip sound;
    private AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public bool Interact() {
        audioSource.clip = sound;
        audioSource.Play();
        return true;
    }

}
