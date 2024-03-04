using UnityEngine;

public abstract class DayBaseState {

    public abstract void EnterState(DayManager context);
    public abstract void UpdateState(DayManager context);
    public abstract bool CanMobSpawn();

}
