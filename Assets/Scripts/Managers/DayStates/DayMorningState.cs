using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Morning State goes from 06:00 to 14:00 where we switch to Afternoon State
/// </summary>
public class DayMorningState : DayBaseState {

    private const float AFTERNOON_THRESHOLD = 28800f; //8-Hours in seconds from start-of-day

    public override void EnterState(DayManager context) {
        context.StartDay();
    }

    public override void UpdateState(DayManager context) {
        if(context.GetCurrentGameTime() >= AFTERNOON_THRESHOLD) {
            //8-hours passed -> switch to afternoon
            context.SwitchState(context.afternoonState);
        }
    }

    public override bool CanMobSpawn() {
        return false;
    }

}
