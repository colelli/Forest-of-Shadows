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
        AudioManager.Instance.PlayMorningSounds();

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
        Light light = context.GetGlobalLight();
        GameDayGraphicsData morningGraphicsData = context.GetMorningGraphicsData();
        light.color = morningGraphicsData.GetLightColour();
        light.colorTemperature = morningGraphicsData.GetLightTemperature();
        light.intensity = morningGraphicsData.GetLightIntensity();

        // Volume setup
        ColorAdjustments colourAdjustments = context.GetColorAdjustments();
        colourAdjustments.postExposure.SetValue(new UnityEngine.Rendering.FloatParameter(morningGraphicsData.GetVolumeExposure()));
        colourAdjustments.colorFilter.SetValue(new UnityEngine.Rendering.ColorParameter(morningGraphicsData.GetVolumeTint()));
    }

    protected void UpdateLightAndVolume(DayManager context) {
        float ratio = context.GetCurrentGameTime() / AFTERNOON_THRESHOLD;

        // Light update
        Light light = context.GetGlobalLight();
        GameDayGraphicsData morningGraphicsData = context.GetMorningGraphicsData();
        GameDayGraphicsData afternoonGraphicsData = context.GetAfternoonGraphicsData();
        light.color = Color.Lerp(morningGraphicsData.GetLightColour(), afternoonGraphicsData.GetLightColour(), ratio);
        light.colorTemperature = Mathf.Lerp(morningGraphicsData.GetLightTemperature(), afternoonGraphicsData.GetLightTemperature(), ratio);
        light.intensity = Mathf.Lerp(morningGraphicsData.GetLightIntensity(), afternoonGraphicsData.GetLightIntensity(), ratio);

        // Volume update
        ColorAdjustments colourAdjustments = context.GetColorAdjustments();
        colourAdjustments.postExposure.SetValue(new UnityEngine.Rendering.FloatParameter(Mathf.Lerp(morningGraphicsData.GetVolumeExposure(), afternoonGraphicsData.GetVolumeExposure(), ratio)));
        colourAdjustments.colorFilter.SetValue(new UnityEngine.Rendering.ColorParameter(Color.Lerp(morningGraphicsData.GetVolumeTint(), afternoonGraphicsData.GetVolumeTint(), ratio)));
    }

}
