using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider))]
public abstract class Enemy : MonoBehaviour, IEnemy {

    protected Transform target;
    protected Vector3 targetPosition;

    [SerializeField] protected EnemySO enemyType;
    [SerializeField] private AudioClip[] mobNoises;
    [SerializeField] private AudioClip[] onDeathSounds;
    protected AudioSource audioSource;
    protected Collider mobCollider;
    protected float currentHealth;

    private const float mobPlayNoiseUpperBound = 60f;
    private const float mobPlayNoiseLowerBound = 25f;
    private float mobPlayNoiseInterval;

    protected void Awake() {
        audioSource = GetComponent<AudioSource>();
        currentHealth = enemyType.enemyMaxHealth;
    }

    protected void Start() {
        target = FindObjectOfType<Player>().transform;
        targetPosition = target.position;
        mobPlayNoiseInterval = Random.Range(mobPlayNoiseLowerBound, mobPlayNoiseUpperBound);
    }

    protected void Update() {

        targetPosition = target.position;

        if (mobPlayNoiseInterval <= 0f) {
            //We play a random noise every random seconds
            mobPlayNoiseInterval = Random.Range(mobPlayNoiseLowerBound, mobPlayNoiseUpperBound);
            PlayNoise(mobNoises[Random.Range(0, mobNoises.Length)]);
        } else {
            mobPlayNoiseInterval -= Time.deltaTime;
        }

    }

    public void DealDamage(Player player) {
        player.TakeDamage(enemyType.attackDamage);
    }

    public void TakeDamage(float dmgAmount) {
        if(currentHealth <= dmgAmount) {
            //Mob health isn't enough to sustain the dmg
            Death();
        } else {
            currentHealth -= dmgAmount;
        }
    }

    private void Death() {
        PlayNoise(onDeathSounds[Random.Range(0, onDeathSounds.Length)]);
        Destroy(gameObject);
    }

    protected void PlayNoise(AudioClip noise) {
        audioSource.PlayOneShot(noise);
    }

    public abstract bool IsSensibleToLight();
    protected abstract void Move();

}