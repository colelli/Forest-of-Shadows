using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider))]
public abstract class Enemy : MonoBehaviour, IEnemy {

    [Header("Target configs")]
    [SerializeField] [Tooltip("Mob default target transform")] protected Transform target;

    [Header("Mob Configs")]
    [SerializeField] [Tooltip("EnemySO default type to retrieve statistics from")] protected EnemySO enemyType;
    private bool isStunned;
    [SerializeField] [Min(0)] private float defaultMaxStunDuration;
    private float stunDuration;
    protected float currentHealth;
    protected Collider mobCollider;
    protected Rigidbody rb;

    [Header("Mob Audio Configs")]
    [SerializeField] private AudioClip[] mobNoises;
    [SerializeField] private AudioClip[] onDeathSounds;
    protected AudioSource audioSource;

    private const float mobPlayNoiseUpperBound = 60f;
    private const float mobPlayNoiseLowerBound = 25f;
    private float mobPlayNoiseInterval;

    protected void Awake() {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        currentHealth = enemyType.enemyMaxHealth;
        isStunned = false;
    }

    protected void Start() {
        mobPlayNoiseInterval = Random.Range(mobPlayNoiseLowerBound, mobPlayNoiseUpperBound);
    }

    protected void Update() {

        if (mobPlayNoiseInterval <= 0f) {
            //We play a random noise every random seconds
            mobPlayNoiseInterval = Random.Range(mobPlayNoiseLowerBound, mobPlayNoiseUpperBound);
            //PlayNoise(mobNoises[Random.Range(0, mobNoises.Length)]);
        } else {
            mobPlayNoiseInterval -= Time.deltaTime;
        }

        isStunned = stunDuration > 0f;
        if(!isStunned) {
            //Mob can move
            Move();
        } else {
            //Mob is still stunned -> reduce remaining duration
            stunDuration -= Time.deltaTime;
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
        //PlayNoise(onDeathSounds[Random.Range(0, onDeathSounds.Length)]);
        Destroy(gameObject);
    }

    protected void PlayNoise(AudioClip noise) {
        audioSource.PlayOneShot(noise);
    }

    protected void StunSelf() {
        stunDuration = defaultMaxStunDuration;
    }

    public abstract bool IsSensibleToLight();
    protected abstract void Move();

}