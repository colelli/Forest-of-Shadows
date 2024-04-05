using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
        Light light = context.GetGlobalLight();
        GameDayGraphicsData afternoonGraphicsData = context.GetAfternoonGraphicsData();
        light.color = afternoonGraphicsData.GetLightColour();
        light.colorTemperature = afternoonGraphicsData.GetLightTemperature();
        light.intensity = afternoonGraphicsData.GetLightIntensity();

        // Volume setup
        ColorAdjustments colourAdjustments = context.GetColorAdjustments();
        colourAdjustments.postExposure.SetValue(new UnityEngine.Rendering.FloatParameter(afternoonGraphicsData.GetVolumeExposure()));
        colourAdjustments.colorFilter.SetValue(new UnityEngine.Rendering.ColorParameter(afternoonGraphicsData.GetVolumeTint()));
    }

    private void UpdateLightAndVolume(DayManager context) {
        float ratio = (context.GetCurrentGameTime() - TIME_ELAPSED_TILL_START_OF_STATE) / (NIGHT_THRESHOLD - TIME_ELAPSED_TILL_START_OF_STATE);

        // Light update
        Light light = context.GetGlobalLight();
        GameDayGraphicsData afternoonGraphicsData = context.GetAfternoonGraphicsData();
        GameDayGraphicsData nightGraphicsData = context.GetNightGraphicsData();
        light.color = Color.Lerp(afternoonGraphicsData.GetLightColour(), nightGraphicsData.GetLightColour(), ratio);
        light.colorTemperature = Mathf.Lerp(afternoonGraphicsData.GetLightTemperature(), nightGraphicsData.GetLightTemperature(), ratio);
        light.intensity = Mathf.Lerp(afternoonGraphicsData.GetLightIntensity(), nightGraphicsData.GetLightIntensity(), ratio);

        // Volume update
        ColorAdjustments colourAdjustments = context.GetColorAdjustments();
        colourAdjustments.postExposure.SetValue(new UnityEngine.Rendering.FloatParameter(Mathf.Lerp(afternoonGraphicsData.GetVolumeExposure(), nightGraphicsData.GetVolumeExposure(), ratio)));
        colourAdjustments.colorFilter.SetValue(new UnityEngine.Rendering.ColorParameter(Color.Lerp(afternoonGraphicsData.GetVolumeTint(), nightGraphicsData.GetVolumeTint(), ratio)));
    }

}
