using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class LightSwitchInteraction : MonoBehaviour
{
    public static event Action<LightSwitchInteraction, bool> LightsToggle;

    private XRSimpleInteractable interactable;

    [Header("Lights to turn on")]
    public Light[] shelterLights;
    private bool lightsOn = false;

    private void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();

        lightsOn = false;
        foreach (Light l in shelterLights) 
        {
            l.enabled = false;
        }
    }

    private void OnEnable()
    {
        interactable.selectEntered.AddListener(OnSelected);
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnSelected);
    }

    private void OnSelected(SelectEnterEventArgs args) 
    {
        ActivateSwitch();
    }
    public void ActivateSwitch() 
    {
        lightsOn = !lightsOn;
        
        foreach (Light L in shelterLights) 
        {
            L.enabled = lightsOn;
        }
        LightsToggle?.Invoke(this, lightsOn);

       
    }
}


