using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Morning State goes from 06:00 to 14:00 where we switch to Afternoon State
/// </summary>
public class DayMorningState : DayBaseState {

    private const float AFTERNOON_THRESHOLD = 28800f; //8-Hours in seconds from start-of-day

    public override void EnterState(DayManager context) {
        SetupLightAndVolume(context);
        context.StartDay();

        Debug.Log($"[{context.GetType()}] >>> Morning started");
    }

    public override void UpdateState(DayManager context) {
        UpdateLightAndVolume(context);
        if (context.GetCurrentGameTime() >= AFTERNOON_THRESHOLD) {
            //8-hours passed -> switch to afternoon
            context.SwitchState(context.afternoonState);
        }
    }

    protected override void SetupLightAndVolume(DayManager context) {
        // Light setup
        context.sceneLight.color = context.morningDayGraphicsData.GetLightColour();
        context.sceneLight.colorTemperature = context.morningDayGraphicsData.GetLightTemperature();
        context.sceneLight.intensity = context.morningDayGraphicsData.GetLightIntensity();

        // Volume setup
        context.colorAdjustments.postExposure.SetValue(new UnityEngine.Rendering.FloatParameter(context.morningDayGraphicsData.GetVolumeExposure()));
        context.colorAdjustments.colorFilter.SetValue(new UnityEngine.Rendering.ColorParameter(context.morningDayGraphicsData.GetVolumeTint()));
    }

    protected void UpdateLightAndVolume(DayManager context) {
        float ratio = context.GetCurrentGameTime() / AFTERNOON_THRESHOLD;

        // Light update
        context.sceneLight.color = Color.Lerp(context.morningDayGraphicsData.GetLightColour(), context.afternoonDayGraphicsData.GetLightColour(), ratio);
        context.sceneLight.colorTemperature = Mathf.Lerp(context.morningDayGraphicsData.GetLightTemperature(), context.afternoonDayGraphicsData.GetLightTemperature(), ratio);
        context.sceneLight.intensity = Mathf.Lerp(context.morningDayGraphicsData.GetLightIntensity(), context.afternoonDayGraphicsData.GetLightIntensity(), ratio);

        // Volume update
        context.colorAdjustments.postExposure.SetValue(new UnityEngine.Rendering.FloatParameter(Mathf.Lerp(context.morningDayGraphicsData.GetVolumeExposure(), context.afternoonDayGraphicsData.GetVolumeExposure(), ratio)));
        context.colorAdjustments.colorFilter.SetValue(new UnityEngine.Rendering.ColorParameter(Color.Lerp(context.morningDayGraphicsData.GetVolumeTint(), context.afternoonDayGraphicsData.GetVolumeTint(), ratio)));
    }

}
