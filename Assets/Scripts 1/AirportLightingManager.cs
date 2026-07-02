using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AirportLightingManager : MonoBehaviour
{
    public static AirportLightingManager Instance;

    [Header("Runway Lights Intensity")]
    public float runwayLightsIntensity = 5.0f;

    [Header("Runway Emission Intensity")]
    public float runwayEmissionIntensity = 2.0f;

    private readonly List<RunwayLights> runwayLights = new();
    void Awake()
    {
        Instance = this;
    }

    public void RegisterRunwayLight(RunwayLights light) 
    {
        runwayLights.Add(light);
        light.ApplySettings(runwayLightsIntensity, runwayEmissionIntensity);
    }

    public void UnRegisterRunwayLight(RunwayLights light) 
    {
        runwayLights.Remove(light);
    }

    public void UpdateRunwayLights() 
    {
        foreach (var light in runwayLights) 
        {
            light.ApplySettings(runwayLightsIntensity, runwayEmissionIntensity);
        }   
    }


#if UNITY_EDITOR
    void OnValidate()
    {
        if (!Application.isPlaying) return;

        UpdateRunwayLights();
    }

#endif
}
