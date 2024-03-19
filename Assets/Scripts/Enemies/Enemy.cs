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

    private float attackDamage;

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
        if (target == null) {
            //A target has not been assigned -> We default it to the Player
            FindTarget();
        }
        mobPlayNoiseInterval = Random.Range(mobPlayNoiseLowerBound, mobPlayNoiseUpperBound);
        attackDamage = enemyType.baseAttackDamage * GameManager.Instance.GetCurrentDifficultyData().GetDifficultyMultiplier();
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

    protected void FindTarget() {
        target = FindObjectOfType<Player>().transform;
    }

    public void DealDamage(Player player) {
        player.TakeDamage(attackDamage);
    }

    public void TakeDamage(float dmgAmount) {
        if(currentHealth <= dmgAmount) {
            //Mob health isn't enough to sustain the dmg
            Death();
        } else {
            currentHealth -= dmgAmount;
        }
    }

    protected void Death() {
        //PlayNoise(onDeathSounds[Random.Range(0, onDeathSounds.Length)]);
        EnemySpawnManager.Instance.ReducePowerLevel(enemyType);
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