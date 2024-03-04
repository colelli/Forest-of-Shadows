using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Night State goes from 20:00 until the player either goes back to the "base" or fails the surviving the night
/// </summary>
public class DayNightState : DayBaseState {

    private const float AFTERNOON_THRESHOLD = 28800f; //8-Hours in seconds

    public override void EnterState(DayManager context) {
        //TO-DO
    }

    public override void UpdateState(DayManager context) {
        //TO-DO
    }

    public override bool CanMobSpawn() {
        return true;
    }

}
