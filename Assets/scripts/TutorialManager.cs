using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TutorialManager : MonoBehaviour
{
    public TutorialStep currentState;

    [Header("Highlights")]
    public HighlightController doorKnob;
    public HighlightController lightSwitch;
    public HighlightController localiserKnob;
    public HighlightController distributionBoard;

    [Header("Step UI Prefabs")]
    public GameObject outsideShelterUI;
    public GameObject distributionBoardUI;
   

    private GameObject currentUI;
    private TutorialStep nextStep;

    [Header("Interactions")]
    public LightSwitchInteraction lightSwitchInteraction;

    void Start()
    {
        DoorInteraction.DoorOpened += HandleDoorOpened;
        LightSwitchInteraction.LightsToggle += HandleLightsToggled;
       // CloseBtn.CloseBtnPressed += HandleCloseBtnPressed;

        SetState(TutorialStep.OutsideShelter);
    }

    private void OnDestroy()
    {
        DoorInteraction.DoorOpened -= HandleDoorOpened;
        LightSwitchInteraction.LightsToggle -= HandleLightsToggled;
        //CloseBtn.CloseBtnPressed -= HandleCloseBtnPressed;
    }

    // ===================== STATE MANAGEMENT =====================

    public void SetState(TutorialStep newState)
    {
        currentState = newState;

        // Reset highlights
        doorKnob.SetHighlight(false);
        lightSwitch.SetHighlight(false);
        localiserKnob.SetHighlight(false);

        HideCurrentUI();

        switch (currentState)
        {
            case TutorialStep.OutsideShelter:
                doorKnob.SetHighlight(true);
                ShowUI(outsideShelterUI);
                nextStep = TutorialStep.TurnOnLights;
                break;

            case TutorialStep.TurnOnLights:
                lightSwitch.SetHighlight(true);
                nextStep = TutorialStep.DistributionBoard;
                break;

            case TutorialStep.DistributionBoard:
                distributionBoard.SetHighlight(true);
                ShowUI(distributionBoardUI);
                nextStep = TutorialStep.FireExtinguishers;
                break;

            case TutorialStep.HighlightLocaliserKnob:


                break;
        }
    }

    // ===================== UI SYSTEM =====================

    private void ShowUI(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("UI prefab is null!");
            return;
        }

        currentUI = Instantiate(prefab);

        // No positioning logic ? prefab keeps its designed position
        currentUI.SetActive(true);
        XRBaseInteractable xrButton = currentUI.GetComponentInChildren<XRBaseInteractable>(true);

        if (xrButton != null)
        {
            xrButton.selectEntered.RemoveAllListeners();
            xrButton.selectEntered.AddListener(_ => OnUIClosePressed());
        }
    }

    private void HideCurrentUI()
    {
        if (currentUI != null)
        {
            Destroy(currentUI);
            currentUI = null;
        }
    }

    private void OnUIClosePressed() 
    {
        HideCurrentUI();
        SetState(nextStep);
    }

    // ===================== EVENTS =====================

    private void HandleDoorOpened(DoorInteraction door)
    {
        if (currentState != TutorialStep.OutsideShelter)
            return;

        SetState(TutorialStep.TurnOnLights);
    }

    private void HandleLightsToggled(LightSwitchInteraction swh, bool isOn)
    {
        if (swh != lightSwitchInteraction)
            return;

        if (currentState == TutorialStep.TurnOnLights && isOn)
        {
            SetState(TutorialStep.DistributionBoard);
        }
    }

    //private void HandleCloseBtnPressed(CloseBtn button)
    //{
    //    HideCurrentUI();

    //    if (currentState == TutorialStep.LocaliserOverView)
    //    {
    //        SetState(TutorialStep.HighlightLocaliserKnob);
    //    }
    //}
}