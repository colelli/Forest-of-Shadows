using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Night State goes from 20:00 until the player either goes back to the "base" or fails the surviving the night
/// </summary>
public class DayNightState : DayBaseState {

    private float currentSpawnTimer;
    private const float maxDebuffTimer = 60f;
    private float debuffTimer;

    public override void EnterState(DayManager context) {
        debuffTimer = maxDebuffTimer;
        currentSpawnTimer = 999f;
        Debug.Log($"[{context.GetType()}] >>> Night started");
    }

    /// <summary>
    /// During the night, we check if we can spawn a new enemy every <c>enemySpawnInterval</c>.<br/>
    /// If the current timer is greater or equal to that we first reset it, and spawn a new enemy.
    /// </summary>
    /// <param name="context"></param>
    public override void UpdateState(DayManager context) {

        ReducePlayerSanity();
        TrySpawnEnemy();

    }

    private void ReducePlayerSanity() {
        if(debuffTimer <= 0) {
            //We can apply the debuff
            GameManager.Instance.GetPlayer().DrainSanity();
            debuffTimer = maxDebuffTimer;
        }
        debuffTimer -= Time.deltaTime;
    }

    private void TrySpawnEnemy() {
        if (!EnemySpawnManager.Instance.CanSpawnNewEnemy()) return;

        if (currentSpawnTimer >= GameManager.Instance.GetCurrentDifficultyData().GetEnemySpawnInterval()) {
            currentSpawnTimer = 0f;
            EnemySpawnManager.Instance.SpawnRandomEnemy();
        }
        currentSpawnTimer += Time.deltaTime;
    }

}
