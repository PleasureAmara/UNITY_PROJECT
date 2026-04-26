using System.Linq;
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
    public HighlightController statusPanel;
    public HighlightController alertButton;
    public HighlightController localiserDoor;
    public HighlightController[] powerAmplifiers1;
    public HighlightController[] powerAmplifiers2;

    [Header("Step UI Prefabs")]
    public GameObject outsideShelterUI;
    public GameObject distributionBoardUI;
    public GameObject fireExtinguishersUI;
    public GameObject statusPanelUI;
    public GameObject alertButtonUI;
    public GameObject localiserDoorUI;
    public GameObject powerAmplifiers1UI;
    public GameObject powerAmplifiers2UI;


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
        distributionBoard.SetHighlight(false);
        statusPanel.SetHighlight(false);
        alertButton.SetHighlight(false);
        localiserDoor.SetHighlight(false);
       foreach(var pa in powerAmplifiers1)
        {
            pa.SetHighlight(false);
        }

       foreach (var pa in powerAmplifiers2)
        {
            pa.SetHighlight(false);
        }


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
                nextStep = TutorialStep.StatusPanel;
                break;


            case TutorialStep.StatusPanel:
                statusPanel.SetHighlight(true);
                ShowUI(statusPanelUI);
                nextStep = TutorialStep.AlertsButton;
                break;

            case TutorialStep.AlertsButton:
                alertButton.SetHighlight(true);
                ShowUI(alertButtonUI);
                nextStep = TutorialStep.LocaliserDoor;
                break;

            case TutorialStep.LocaliserDoor:
                localiserDoor.SetHighlight(true);
                ShowUI(localiserDoorUI);
                nextStep = TutorialStep.MODPA1;
                break;

            case TutorialStep.MODPA1:
                foreach (var pa in powerAmplifiers1)
                {
                    pa.SetHighlight(true);
                }

                ShowUI(powerAmplifiers1UI);
                nextStep = TutorialStep.MODPA2;
                break;

            case TutorialStep.MODPA2:
                foreach (var pa in powerAmplifiers2)
                {
                    pa.SetHighlight(true);
                }

                ShowUI(powerAmplifiers2UI);
                nextStep = TutorialStep.ECU;
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