using localizer.core.enums;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

//local imports
using localizer.product.descriptions;
using localizer.product.player;



/*##########################################################################
  #                                                                        #
  #             for lear*ning how to use controllers.                       #
  #                                                                        #
  ##########################################################################*/
[Serializable]
public class LearnVRControllers
{
    //for target gameobjects
    public HighlightController grip;
    public HighlightController snapTurn;
    public HighlightController MoveThumbstick;
    //public HighlightController metaQuestButton;

    public Canvas itemsCanvas;
    public TextMeshProUGUI title;
    public TextMeshProUGUI content;
    public XRSimpleInteractable continueButton;
    public XRSimpleInteractable exitIntroButton;

    public TrackerUserInput trackUserInput;

    public CharacterController xrOriginCharacterController;

    /// <summary>
    /// Represents the true value stored that is matching the active controller key expected from the user. 
    /// It's used whenever we call the method ActionsDescriptions.FindDescription()
    /// </summary>
    [HideInInspector] public TargetControllerKeys actualControllerKey;

}

public class TutorialManager : MonoBehaviour
{
    public TutorialStep currentState;

    [Header("Highlights")]
    public HighlightController doorKnob;
    public HighlightController lightSwitch;
    public HighlightController localiserKnob;
    public HighlightController[] distributionBoard;
    public HighlightController statusPanel;
    public HighlightController alertButton;
    public HighlightController localiserDoor;
    public HighlightController[] powerAmplifiers1;
    public HighlightController[] powerAmplifiers2;
    public HighlightController interfaceCard;
    public HighlightController[] synthesizers;
    public HighlightController[] audioGenerators;
    public HighlightController[] monitor;
    public HighlightController ecu;
    public HighlightController[] batteries;
    public HighlightController stabilizer;
    public HighlightController awos35;
    public HighlightController airConditioners;
    public HighlightController[] upsBatteries;
   


    [Header("Step UI Prefabs")]
    public GameObject outsideShelterUI;
    public GameObject switchUI;
    public GameObject distributionBoardUI;
    
    public GameObject statusPanelUI;
    public GameObject alertButtonUI;
    public GameObject localiserDoorUI;
    public GameObject powerAmplifiers1UI;
    public GameObject powerAmplifiers2UI;
    public GameObject interfaceCardUI;
    public GameObject synthesizersUI;
    public GameObject audioGeneratorsUI;
    public GameObject monitorUI;
    public GameObject ecuUI;
    public GameObject batteriesUI;
    public GameObject fireExtinguishersUI;
    public GameObject stabilizerUI;
    public GameObject awos35UI;
    public GameObject airConditionerUI;
    public GameObject upsBatteriesUI;
    
    private GameObject currentUI;
    private TutorialStep nextStep;

    [Header("Interactions")]
    public LightSwitchInteraction lightSwitchInteraction;


    //the learn vr class
    [SerializeField] private LearnVRControllers learnVRControllers;

    /// <summary>
    /// the role is to notify all callback methods that should start only after the learn VR Tutorials are finished.
    /// </summary>
    [HideInInspector] public event Action isLearnVRFinished;

    private bool introActive;

    private void OnEnable()
    {
        learnVRControllers.trackUserInput.OnUserPressed += ManageUserActions;
        learnVRControllers.continueButton.selectEntered.RemoveAllListeners();
        learnVRControllers.continueButton.selectEntered.AddListener(ManageContinueButton);
    }

    private void OnDisable()
    {
        learnVRControllers.trackUserInput.OnUserPressed -= ManageUserActions;
        learnVRControllers.continueButton.selectEntered.RemoveAllListeners();

    }


    void Start()
    {
        
        LightSwitchInteraction.LightsToggle += HandleLightsToggled;
        //SetState(TutorialStep.OutsideShelter);

        //Wait for the xrorigin to initialise all its children gameobjects before we SetState()
        StartCoroutine(StartSetState());

        ////============================TODO =========================: Find why the method returns error for xrorigin
        //////objects but not for normal objects.
        //if (learnVRControllers.itemsCanvas != null)  learnVRControllers.itemsCanvas.gameObject.SetActive(true);
        //if (learnVRControllers.continueButton != null) learnVRControllers.continueButton.gameObject.SetActive(false);
    }

    //private void Update()
    //{
    //    if (nextStep == TutorialStep.OutsideShelter)
    //    {
    //        SetState(TutorialStep.OutsideShelter);
    //    }
    //}

    private void OnDestroy()
    {
        LightSwitchInteraction.LightsToggle -= HandleLightsToggled;
       
    }

    // ===================== STATE MANAGEMENT =====================

    public void SetState(TutorialStep newState)
    {
        currentState = newState;

        // Reset highlights
        learnVRControllers.grip.SetHighlight(false);
        learnVRControllers.snapTurn.SetHighlight(false);
        learnVRControllers.MoveThumbstick.SetHighlight(false);
        //learnVRControllers.metaQuestButton.SetHighlight(false);

        doorKnob.SetHighlight(false);
        lightSwitch.SetHighlight(false);
        foreach (var db in distributionBoard)
        {
            db.SetHighlight(false);
        }

        statusPanel.SetHighlight(false);
        alertButton.SetHighlight(false);
        localiserDoor.SetHighlight(false);
        ecu.SetHighlight(false);
        foreach (var pa in powerAmplifiers1)
        {
            pa.SetHighlight(false);
        }

       foreach (var pa in powerAmplifiers2)
        {
            pa.SetHighlight(false);
        }

        foreach (var sy in synthesizers)
        {
            sy.SetHighlight(false);
        }

        foreach (var ag in audioGenerators)
        {
            ag.SetHighlight(false);
        }

        foreach (var mr in monitor)
        {
            mr.SetHighlight(false);
        }

        foreach (var bt in batteries)
        {
            bt.SetHighlight(false);
        }
        stabilizer.SetHighlight(false);
        awos35.SetHighlight(false);
        airConditioners.SetHighlight(false);
        interfaceCard.SetHighlight(false);

        foreach (var bt in upsBatteries)
        {
            bt.SetHighlight(false);
        }
        HideCurrentUI();

        //string associatedContent;
        switch (currentState)
        {

            //learning vr controllers
            case TutorialStep.WelcomeToVR:
                string associatedContent = ActionsDescriptions.FindDescription(TutorialStep.WelcomeToVR, out learnVRControllers.actualControllerKey);
                learnVRControllers.title.text = "Welcome to VR tutorial";
                learnVRControllers.content.text = associatedContent;
                learnVRControllers.exitIntroButton.gameObject.SetActive(false);
                //highlight the button and set stage for the next step
                learnVRControllers.grip.SetHighlight(true);
                nextStep = TutorialStep.LearningGrip;
                break;

            case TutorialStep.LearningGrip:
                associatedContent = ActionsDescriptions.FindDescription(TutorialStep.LearningGrip, out learnVRControllers.actualControllerKey);
                learnVRControllers.title.text = "Selecting buttons and Opening doors";
                learnVRControllers.content.text = associatedContent;
                learnVRControllers.exitIntroButton.gameObject.SetActive(false);
                learnVRControllers.snapTurn.SetHighlight(true);
                nextStep = TutorialStep.LearningTurning;
                break;

            case TutorialStep.LearningTurning:
                associatedContent = ActionsDescriptions.FindDescription(TutorialStep.LearningTurning, out learnVRControllers.actualControllerKey);
                learnVRControllers.title.text = "Turning with controllers.";
                learnVRControllers.content.text = associatedContent;
                learnVRControllers.exitIntroButton.gameObject.SetActive(false);
                learnVRControllers.MoveThumbstick.SetHighlight(true);
                nextStep = TutorialStep.LearningStraightMovt;

                //try
                //learnVRControllers.continueButton.gameObject.SetActive(true);
                //nextStep = TutorialStep.OutsideShelter;
                //SetState(nextStep);
                //try

                break;

            case TutorialStep.LearningStraightMovt:
                associatedContent = ActionsDescriptions.FindDescription(TutorialStep.LearningStraightMovt, out learnVRControllers.actualControllerKey);
                learnVRControllers.title.text = "Straight Locomotion";
                learnVRControllers.content.text = associatedContent;
                learnVRControllers.exitIntroButton.gameObject.SetActive(false);
                nextStep = TutorialStep.FinishVRControllerLearning;
                break;

            case TutorialStep.FinishVRControllerLearning:
                associatedContent = ActionsDescriptions.FindDescription(TutorialStep.FinishVRControllerLearning, out learnVRControllers.actualControllerKey);
                learnVRControllers.title.text = "Conclusion";
                learnVRControllers.content.text = associatedContent;
                learnVRControllers.continueButton.gameObject.SetActive(true);
                learnVRControllers.exitIntroButton.gameObject.SetActive(false);
                introActive = false;
                nextStep = TutorialStep.OutsideShelter;
                //StartOtherTutorials();
                //SetState(TutorialStep.OutsideShelter);
                break;

            case TutorialStep.OutsideShelter:
                Debug.Log("Reached OutsideShelter");

                //Debug.Log("doorKnob = " + doorKnob);
                //Debug.Log("outsideShelterUI = " + outsideShelterUI);
                doorKnob.SetHighlight(true);
                ShowUI(outsideShelterUI);
                nextStep = TutorialStep.TurnOnLights;
                break;

            case TutorialStep.TurnOnLights:
                lightSwitch.SetHighlight(true);
                ShowUI(switchUI);
                nextStep = TutorialStep.DistributionBoard;
                break;

            case TutorialStep.DistributionBoard:
                foreach (var db in distributionBoard)
                {
                    db.SetHighlight(true);
                }
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
                nextStep = TutorialStep.interfaceCard;
                break;

            case TutorialStep.interfaceCard:
                interfaceCard.SetHighlight(true);
                ShowUI(interfaceCardUI);
                nextStep = TutorialStep.Synthesizers;
                break;


            case TutorialStep.Synthesizers:
                foreach (var sy in synthesizers)
                {
                    sy.SetHighlight(true);
                }

                ShowUI(synthesizersUI);
                nextStep = TutorialStep.AudioGenerators;
                break;

            case TutorialStep.AudioGenerators:
                foreach (var ag in audioGenerators)
                {
                    ag.SetHighlight(true);
                }

                ShowUI(audioGeneratorsUI);
                nextStep = TutorialStep.monitor;
                break;


            case TutorialStep.monitor:
                foreach (var mr in monitor)
                {
                    mr.SetHighlight(true);
                }
                ShowUI(monitorUI);
                nextStep = TutorialStep.ECU;
                break;

            case TutorialStep.ECU:
               ecu.SetHighlight(true);
                ShowUI(ecuUI);
                nextStep = TutorialStep.batteries;
                break;



            case TutorialStep.batteries:
                foreach (var bt in batteries)
                {
                    bt.SetHighlight(true);
                }
                ShowUI(batteriesUI);
                nextStep = TutorialStep.FireExtinguishers;
                break;

            case TutorialStep.FireExtinguishers:
                ShowUI(fireExtinguishersUI);
                nextStep = TutorialStep.voltageStabilizer;
                break;

            case TutorialStep.voltageStabilizer:
                stabilizer.SetHighlight(true);
                ShowUI(stabilizerUI);
                nextStep = TutorialStep.awosCabinet;
                break;

            case TutorialStep.awosCabinet:
                awos35.SetHighlight(true);
                ShowUI(awos35UI);
                nextStep = TutorialStep.AirConditioners;
                break;

            case TutorialStep.AirConditioners:
                airConditioners.SetHighlight(true);
                ShowUI(airConditionerUI);
                nextStep = TutorialStep.upsBatteries;
                break;

            case TutorialStep.upsBatteries:
                foreach (var bt in upsBatteries)
                {
                    bt.SetHighlight(true);
                }
                ShowUI(upsBatteriesUI);
                nextStep = TutorialStep.Completed;
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

  

    private void HandleLightsToggled(LightSwitchInteraction swh, bool isOn)
    {
        if (swh != lightSwitchInteraction)
            return;

        if (currentState == TutorialStep.TurnOnLights && isOn)
        {
            SetState(TutorialStep.DistributionBoard);
        }
    }

    /// <summary>
    /// For managing callbacks whenever  a user presses controller key.
    /// </summary>
    /// <param name="pressedKeys"> the List of actions triggered by a key press.</param>
    private void ManageUserActions(string pressedKey)
    {
        if (!introActive) return;

        string actualControllerKeyString = learnVRControllers.actualControllerKey.ToString();
        //Debug.Log($"Actual string:------------> {actualControllerKeyString}");

        //Correct the snap turn string. 
        if (actualControllerKeyString == "SnapTurn") actualControllerKeyString = "Snap Turn";
        if (pressedKey == actualControllerKeyString)
        {
            //Debug.Log("Condition passed. Advancing to next level.");
            SetState(nextStep);

        }

    }

    void StartOtherTutorials()
    {
        Debug.Log("Other tutorials triggered");
        SetState(TutorialStep.OutsideShelter);
    }

    /// <summary>
    /// only triggers when the continue button is clicked.
    /// </summary>
    /// <param name="args"></param>
    private void ManageContinueButton(SelectEnterEventArgs args)
    {
        learnVRControllers.itemsCanvas.gameObject.SetActive(false);
        SetState(TutorialStep.OutsideShelter);
        isLearnVRFinished?.Invoke();
    }

    IEnumerator StartSetState()
    {
        while (learnVRControllers.xrOriginCharacterController.transform.GetComponentInChildren<HighlightController>() == null)
        {
            yield return null;
        }
        introActive = true;
        SetState(TutorialStep.WelcomeToVR);
    }

}