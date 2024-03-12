using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Night State goes from 20:00 until the player either goes back to the "base" or fails the surviving the night
/// </summary>
public class DayNightState : DayBaseState {

    private float currentSpawnTimer;

    public override void EnterState(DayManager context) {
        currentSpawnTimer = 999f;
        Debug.Log($"[{context.GetType()}] >>> Night started");
    }

    /// <summary>
    /// During the night, we check if we can spawn a new enemy every <c>enemySpawnInterval</c>.<br/>
    /// If the current timer is greater or equal to that we first reset it, and spawn a new enemy.
    /// </summary>
    /// <param name="context"></param>
    public override void UpdateState(DayManager context) {

        if (!EnemySpawnManager.Instance.CanSpawnNewEnemy()) return;

        if(currentSpawnTimer >= GameManager.Instance.GetCurrentDifficultyData().GetEnemySpawnInterval()) {
            currentSpawnTimer = 0f;
            EnemySpawnManager.Instance.SpawnRandomEnemy();
        }
        currentSpawnTimer += Time.deltaTime;

    }

}
