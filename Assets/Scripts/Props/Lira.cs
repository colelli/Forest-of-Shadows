using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lira : MonoBehaviour, IInteractable {

    [SerializeField] private AudioClip[] sound;
    private AudioSource audioSource;
    private bool _busy = false;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public bool Interact() {
        _busy = true;
        audioSource.PlayOneShot(sound[Random.Range(0, sound.Length)]);
        StartCoroutine(BusyInteracting());
        // this false indicates that the item can be interacted with again
        return false;
    }

    private IEnumerator BusyInteracting() {
        yield return new WaitForSeconds(0.75f);
        _busy = false;
    }

    public bool IsBusy() {
        return _busy;
    }

}
