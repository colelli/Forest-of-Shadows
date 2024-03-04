using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Afternoon State goes from 14:00 to 20:00 where we switch to Night State
/// </summary>
public class DayAfternoonState : DayBaseState {

    private const float NIGHT_THRESHOLD = 50400; //14-Hours in seconds from start-of-day

    public override void EnterState(DayManager context) {
        //TO-DO
    }

    public override void UpdateState(DayManager context) {
        if (context.GetCurrentGameTime() >= NIGHT_THRESHOLD) {
            //6-hours passed from afternoon -> switch to night
            context.SwitchState(context.nightState);
        }
    }

    public override bool CanMobSpawn() {
        return false;
    }

}
