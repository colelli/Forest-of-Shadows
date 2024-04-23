using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance {  get; private set; }
    public enum Indices {
        AMBIENT,
        CREATURES,
        SFX
    }

    [Header("Ambient Sounds")]
    [SerializeField] public AudioClip[] day;
    [SerializeField] private AudioClip[] night;

    [Header("Creatures")]
    [SerializeField] private AudioClip[] dayCreatures;
    [SerializeField] private AudioClip[] nightCreatures;

    /*
     * Audio Source mappings (per index):
     * [0] = Ambient Sounds
     * [1] = Creatures
     * [2] = SFX
     */
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

    public void PlayMorningSounds() {
        // Set sounds
        //audioSources[0].clip = GetRandomDayAmbient();
        audioSources[1].clip = GetRandomDayCreatureSound();

        // Play sounds
        //audioSources[0].Play();
        audioSources[1].Play();
    }

    public void PlayNightSounds() {
        // Set sounds
        audioSources[0].clip = GetRandomNightAmbient();
        audioSources[1].clip = GetRandomNightCreatureSound();

        // Play sounds
        audioSources[0].Play();
        audioSources[1].Play();
    }

    public void PlayOneShot(AudioClip clip) {
        audioSources[2].PlayOneShot(clip);
    }

    public void StopAllSounds() {
        foreach(AudioSource source in audioSources) {
            source.Stop();
        }
    }

    public void StopSoundAtIndex(Indices index) {
        audioSources[(int)index].Stop();
    }

    private void GetAllAudioSourceReferences() {
        audioSources = gameObject.GetComponents<AudioSource>();
    }

    private void PlayAudios() {
        foreach (AudioSource source in audioSources) {
            if (source.clip != null) {
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
