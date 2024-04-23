using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance {  get; private set; }

    [Header("Ambient Sounds")]
    [SerializeField] public AudioClip[] day;
    [SerializeField] private AudioClip[] night;

    [Header("Creatures")]
    [SerializeField] private AudioClip[] dayCreatures;
    [SerializeField] private AudioClip[] nightCreatures;

    private AudioSource[] audioSources;

    private void Awake() {
        //We check if there is already a Singleton of AudioManager
        if (Instance != null && Instance != this) {
            Destroy(this);
            throw new System.Exception($"[{this.name}] >>> An Instance of this Singleton already exists!");
        } else {
            //There are not instances
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        GetAllAudioSourceReferences();
    }

    private void Start() {
        audioSources[0].clip = GetRandomDayAmbient();
        audioSources[1].clip = GetRandomDayCreatureSound();

        PlayAudios();
    }

    private void GetAllAudioSourceReferences() {
        audioSources = gameObject.GetComponents<AudioSource>();
    }

    private void PlayAudios() {
        foreach(AudioSource source in audioSources) {
            if(source.clip != null) {
                source.Play();
            }
        }
    }

    public void UpdateAudioSourcesVolume(float newVolume) {
        foreach(AudioSource source in audioSources) {
            source.volume = Mathf.Clamp(newVolume, 0f, 1f);
        }
    }

    public AudioClip GetRandomDayAmbient() {
        return day[Random.Range(0, day.Length)];
    }

    public AudioClip GetRandomNightAmbient() {
        return night[Random.Range(0, night.Length)];
    }

    public AudioClip GetRandomDayCreatureSound() {
        return dayCreatures[Random.Range(0, dayCreatures.Length)];
    }

    public AudioClip GetRandomNightCreatureSound() {
        return nightCreatures[Random.Range(0, nightCreatures.Length)];
    }

}
