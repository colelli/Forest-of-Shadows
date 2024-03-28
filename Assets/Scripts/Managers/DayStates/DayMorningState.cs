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

    private Color startOfMorningColour = new Color(255f, 235f, 208f);
    private Color endOfMorningColour = new Color(255f, 255f, 230f);

    private const float startOfMorningTemperature = 3000f;
    private const float endOfMorningTemperature = 5500f;

    private const float startOfMorningIntensity = 3f;
    private const float endOfMorningIntensity = 7f;

    public override void EnterState(DayManager context) {
        context.StartDay();
        //SetupLightAndVolume(context);
        Debug.Log($"[{context.GetType()}] >>> Morning started");
    }

    public override void UpdateState(DayManager context) {
        if (context.GetCurrentGameTime() >= AFTERNOON_THRESHOLD) {
            //8-hours passed -> switch to afternoon
            context.SwitchState(context.afternoonState);
        }
    }

    protected override void SetupLightAndVolume(DayManager context) {
        //light setup
        Light light = context.GetLight();
        light.colorTemperature = startOfMorningTemperature;
        light.color = startOfMorningColour;
        light.intensity = startOfMorningIntensity;
        light.SetLightDirty();

        //volume setup
        /*
        Volume volume = context.GetVolume();
        Debug.Log(volume);
        if(volume.profile.TryGet<ColorAdjustments>(out ColorAdjustments colourAdjustments)) {
            Debug.Log(colourAdjustments);
            colourAdjustments.colorFilter.SetValue(new UnityEngine.Rendering.ColorParameter(startOfMorningColour));
            colourAdjustments.saturation.SetValue(new UnityEngine.Rendering.FloatParameter(10f));
            colourAdjustments.postExposure.SetValue(new UnityEngine.Rendering.FloatParameter(0.5f));
        }
        */
    }

}

// startValue + ( ((currentTime - startTime) * (maxValue - startValue)) / maxTime - startTime )
