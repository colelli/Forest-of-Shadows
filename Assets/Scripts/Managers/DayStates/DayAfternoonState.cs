using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Afternoon State goes from 14:00 to 20:00 where we switch to Night State
/// </summary>
public class DayAfternoonState : DayBaseState {

    private const float NIGHT_THRESHOLD = 50400f; //14-Hours in seconds from start-of-day
    private const float TIME_ELAPSED_TILL_START_OF_STATE = 28800f;

    public override void EnterState(DayManager context) {
        SetupLightAndVolume(context);
        Debug.Log($"[{context.GetType()}] >>> Afternoon started");
    }

    public override void UpdateState(DayManager context) {
        UpdateLightAndVolume(context);
        if (context.GetCurrentGameTime() >= NIGHT_THRESHOLD) {
            //6-hours passed from afternoon -> switch to night
            context.SwitchState(context.nightState);
        }
    }

    protected override void SetupLightAndVolume(DayManager context) {
        // Light setup
        context.sceneLight.color = context.afternoonDayGraphicsData.GetLightColour();
        context.sceneLight.colorTemperature = context.afternoonDayGraphicsData.GetLightTemperature();
        context.sceneLight.intensity = context.afternoonDayGraphicsData.GetLightIntensity();

        // Volume setup
        context.colorAdjustments.postExposure.SetValue(new UnityEngine.Rendering.FloatParameter(context.afternoonDayGraphicsData.GetVolumeExposure()));
        context.colorAdjustments.colorFilter.SetValue(new UnityEngine.Rendering.ColorParameter(context.afternoonDayGraphicsData.GetVolumeTint()));
    }

    private void UpdateLightAndVolume(DayManager context) {
        float ratio = (context.GetCurrentGameTime() - TIME_ELAPSED_TILL_START_OF_STATE) / (NIGHT_THRESHOLD - TIME_ELAPSED_TILL_START_OF_STATE);

        // Light update
        context.sceneLight.color = Color.Lerp(context.afternoonDayGraphicsData.GetLightColour(), context.nightDayGraphicsData.GetLightColour(), ratio);
        context.sceneLight.colorTemperature = Mathf.Lerp(context.afternoonDayGraphicsData.GetLightTemperature(), context.nightDayGraphicsData.GetLightTemperature(), ratio);
        context.sceneLight.intensity = Mathf.Lerp(context.afternoonDayGraphicsData.GetLightIntensity(), context.nightDayGraphicsData.GetLightIntensity(), ratio);

        // Volume update
        context.colorAdjustments.postExposure.SetValue(new UnityEngine.Rendering.FloatParameter(Mathf.Lerp(context.afternoonDayGraphicsData.GetVolumeExposure(), context.nightDayGraphicsData.GetVolumeExposure(), ratio)));
        context.colorAdjustments.colorFilter.SetValue(new UnityEngine.Rendering.ColorParameter(Color.Lerp(context.afternoonDayGraphicsData.GetVolumeTint(), context.nightDayGraphicsData.GetVolumeTint(), ratio)));
    }

}
