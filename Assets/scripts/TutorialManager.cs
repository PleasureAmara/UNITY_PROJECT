using TMPro;
using UnityEditor;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public TutorialStep currentState;

    [Header("Highlights")]
    public HighlightController doorKnob;
    public HighlightController lightSwitch;
    public HighlightController localiserKnob;


    [Header("Prompt System")]
    public GameObject promptPrefab;
    public GameObject localiserPrompt;
    private GameObject currentPrompt;

    [Header("Interactions")]
    public LightSwitchInteraction lightSwitchInteraction;
   
    [Header("UI Panels")]
    public GameObject localiserUIPanels;
    private GameObject currentInfoScreen;


    private HighlightController currentHighlightTarget;
    //private TextMeshProUGUI promptTexts;



    void Start()
    {
        ProximityTrigger.PlayerEntered += HandlePlayerEntered;
        ProximityTrigger.PlayerExited += HandlePlayerExited;
        LightSwitchInteraction.LightsToggle += HandleLightsToggled;
        CloseBtn.CloseBtnPressed += HandleCloseBtnPressed;



        SetState(TutorialStep.OutsideShelter);

    }

    private void OnDestroy()
    {
        ProximityTrigger.PlayerEntered -= HandlePlayerEntered;
        ProximityTrigger.PlayerExited -= HandlePlayerExited;
        LightSwitchInteraction.LightsToggle -= HandleLightsToggled;
        CloseBtn.CloseBtnPressed -= HandleCloseBtnPressed;
    }

    private void HandlePlayerEntered(ProximityTrigger trigger)
    {
        if (currentHighlightTarget == null)
            return;

        HighlightController highlight =
            trigger.GetComponentInParent<HighlightController>();

        if (highlight == currentHighlightTarget)
        {
            currentHighlightTarget.SetHighlight(true);
            //ShowPrompt(currentHighlightTarget.transform);
            currentPrompt = Instantiate(promptPrefab);
        }

    }

    private void HandlePlayerExited(ProximityTrigger trigger)
    {
        if (currentHighlightTarget == null)
            return;

        HighlightController highlight =
            trigger.GetComponentInParent<HighlightController>();

        if (highlight == currentHighlightTarget)
        {
            currentHighlightTarget.SetHighlight(false);
            HidePrompt();
        }

        if (currentState == TutorialStep.OutsideShelter && highlight == doorKnob)
        {
            SetState(TutorialStep.TurnOnLights);
        }
    }

    public void SetState(TutorialStep newState)
    {
        currentState = newState;

        // Reset everything
        doorKnob.SetHighlight(false);
        lightSwitch.SetHighlight(false);
        localiserKnob.SetHighlight(false);
        HidePrompt();

        currentHighlightTarget = null;
        

        switch (currentState)
        {
            case TutorialStep.OutsideShelter:
                currentHighlightTarget = doorKnob;
                break;

            case TutorialStep.TurnOnLights:
                currentHighlightTarget = lightSwitch;
                currentHighlightTarget.SetHighlight(true);
                break;

            case TutorialStep.LocaliserOverView:
                break;

            case TutorialStep.HighlightLocaliserKnob:
                currentHighlightTarget = localiserKnob;
                currentHighlightTarget.SetHighlight(true);
                break;


        }
    }

    //private void ShowPrompt(Transform target)
    //{
    //    if (promptPrefab == null)
    //    {
    //        Debug.LogError("Prompt Prefab not assigned.");
    //        return;
    //    }

    //    currentPrompt = Instantiate(promptPrefab);
    //}

    private void HidePrompt()
    {
        if (currentPrompt != null)
        {
            Destroy(currentPrompt);
            currentPrompt = null;
        }
    }


    private void HandleLightsToggled(LightSwitchInteraction swh, bool isOn)
    {
        if (swh != lightSwitchInteraction)
            return;

        if (currentState == TutorialStep.TurnOnLights && isOn)
        {
            SetState(TutorialStep.LocaliserOverView);
            //ShowPrompt(currentHighlightTarget.transform);
            currentPrompt = Instantiate(localiserPrompt);

        }
    }

    private void HandleCloseBtnPressed(CloseBtn button) 
    {
        //if (currentInfoScreen == null)
        //    return;

        //Destroy(currentInfoScreen);
        //currentInfoScreen = null;

        HidePrompt();
        if (currentState == TutorialStep.LocaliserOverView) 
        {
            SetState(TutorialStep.HighlightLocaliserKnob);
        }
    }
}