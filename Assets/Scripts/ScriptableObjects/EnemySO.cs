using UnityEngine;

[CreateAssetMenu()]
public class EnemySO : ScriptableObject {

    [Header("Enemy Visual Configs")]
    public Transform enemyPrefab;
    public Sprite enemyIcon;
    public string enemyName;

    [Header("Enemy Perk Configs")]
    [Range(10f, 200f)] [Min(10f)] public float enemyMaxHealth;
    [Range(0f, 10f)] public float enemyMaxMovementSpeed;
    [Min(1)] public int enemyPowerLevel;
    [Min(0)] public float baseAttackDamage;

}
