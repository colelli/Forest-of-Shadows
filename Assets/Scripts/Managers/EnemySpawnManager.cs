using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour {

    public static EnemySpawnManager Instance { get; private set; }

    [SerializeField] private EnemyListSO enemyList;
    private int maxPowerLevel;
    private int currentPowerLevel;

    private bool canSpawnNewEnemy;

    private void Awake() {
        //We check if there is already a Singleton of EnemySpawnManager
        if (Instance != null && Instance != this) {
            Destroy(this);
            throw new System.Exception("[{$this.name}] >>> An Instance of this Singleton already exists!");
        } else {
            //There are not instances
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start() {
        currentPowerLevel = 0;
        maxPowerLevel = GameManager.Instance.GetCurrentDifficultyData().GetDifficultyMaxPwrLevel();
        canSpawnNewEnemy = true;
    }

    public void SpawnRandomEnemy() {

        if(currentPowerLevel < maxPowerLevel) {
            //We still have 'space' for other mobs to be spawned

            EnemySO enemy = enemyList.enemySOList[Random.Range(0, enemyList.enemySOList.Count)];
            if(currentPowerLevel + enemy.enemyPowerLevel <= maxPowerLevel) {
                //We can spawn the current enemy
                Instantiate(enemy, Vector3.zero, Quaternion.Euler(Vector3.zero));
                currentPowerLevel += enemy.enemyPowerLevel;
            }

        } else {
            canSpawnNewEnemy = false;
        }

    }

    public bool CanSpawnNewEnemy() {
        return canSpawnNewEnemy;
    }

}
