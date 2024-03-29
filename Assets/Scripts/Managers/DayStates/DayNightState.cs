using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Night State goes from 20:00 until the player either goes back to the "base" or fails the surviving the night
/// </summary>
public class DayNightState : DayBaseState {

    private float currentSpawnTimer;
    private float debuffTimer;

    public override void EnterState(DayManager context) {
        SetupLightAndVolume(context);
        currentSpawnTimer = 999f;
        Debug.Log($"[{context.GetType()}] >>> Night started");
    }

    /// <summary>
    /// During the night, we check if we can spawn a new enemy every <c>enemySpawnInterval</c>.<br/>
    /// If the current timer is greater or equal to that we first reset it, and spawn a new enemy.
    /// </summary>
    /// <param name="context"></param>
    public override void UpdateState(DayManager context) {
        ReducePlayerSanity(context);
        TrySpawnEnemy(context);
    }

    private void ReducePlayerSanity(DayManager context) {
        if(debuffTimer <= 0) {
            //We can apply the debuff
            GameManager.Instance.GetPlayer()?.DrainSanity();
            debuffTimer = context.gameDifficultyData.GetSanityDebuffInterval();
        }
        debuffTimer -= Time.deltaTime;
    }

    private void TrySpawnEnemy(DayManager context) {
        if (!EnemySpawnManager.Instance.CanSpawnNewEnemy()) return;

        if (currentSpawnTimer >= context.gameDifficultyData.GetEnemySpawnInterval()) {
            currentSpawnTimer = 0f;
            EnemySpawnManager.Instance.SpawnRandomEnemy();
        }
        currentSpawnTimer += Time.deltaTime;
    }

    protected override void SetupLightAndVolume(DayManager context) {
        // Light setup
        context.sceneLight.color = context.nightDayGraphicsData.GetLightColour();
        context.sceneLight.colorTemperature = context.nightDayGraphicsData.GetLightTemperature();
        context.sceneLight.intensity = context.nightDayGraphicsData.GetLightIntensity();

        // Volume setup
        context.colorAdjustments.postExposure.SetValue(new UnityEngine.Rendering.FloatParameter(context.nightDayGraphicsData.GetVolumeExposure()));
        context.colorAdjustments.colorFilter.SetValue(new UnityEngine.Rendering.ColorParameter(context.nightDayGraphicsData.GetVolumeTint()));
    }

}
