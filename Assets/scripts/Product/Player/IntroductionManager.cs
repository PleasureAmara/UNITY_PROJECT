using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

using localizer.core.enums;
using localizer.product.descriptions;
using localizer.product.vehicle;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using localizer.core.interfaces;
using localizer.product.ui;

namespace localizer.product.player
{
    [Serializable]
    public class GeneralSettings
    {
        [Tooltip("Drag the Teleport Player component that is attached to this game object.")]
        public TeleportPlayer teleportPlayer;

        public SoundController soundController;

        //[Tooltip("Drag the Teleport gameobject under the XR origin")]
        //public TeleportationProvider teleportationProvider;

        [Tooltip("Drag the show description component attached to this gameobject.")]
        [SerializeField] public ShowDescription showDescription;
    }

    [Serializable]
    public class RotationPivots
    {
        public BasePivot apron5Pivot;
        public BasePivot taxiRunwayPivot;
        public BasePivot runwayAccessRoadPivot;
        public BasePivot accessRdRwyPivot;
        public BasePivot locAntennaPivot;
        //public BasePivot locAntennaToShelterPivot;
        public BasePivot locShelterPivot;
    }


    [Serializable]
    public class SpecificAnchor
    {
        [Header("Introduction anchor")]
        public TeleportationAnchor introAnchor;

        [Header("Final position anchor")]
        public TeleportationAnchor finalAnchor;

        [Header("Vehicle front passenger seat anchor")]
        public TeleportationAnchor passengerSeatAnchor;

        //This isnt type TeleportationAnchor because we arent teleporting XR Origin which has a teleportation provider but 
        //rather a normal gameobject i.e steering Pivot.
        [Header("Vehicle steering anchor")]
        public GameObject steeringAnchor;
    }

    [Serializable]
    public class TargetedAudios
    {
        public AudioSource taxiway;
        public AudioSource runway;
        public AudioSource accessRoad;
        public AudioSource locAntenna;
        public AudioSource locShelter;
        public AudioSource finalStatement;
    }

    [Serializable]
    public class CharacterSettings
    {
        [Tooltip("Attach the XR ORIGIN gameobject")]
        public CharacterController playerController;

    }


    //TODO: TO BE DELETED.
    [Serializable]
    public class LearnVRSettings
    {
        [Tooltip("Drag the 'learn Vr controllers' component attached to this gameobject ")]
        public LearnVRControllers1 learnVRControllers;

        /// <summary>
        /// The introduction screen is used to teach the user how to use controllers.
        /// Why dont we have one single screen? 
        /// this is because the layout is different for either, so choosing one for both isnt ideal.
        /// </summary>
        [Tooltip("Drag the 'introduction screen' under the items canvas game object")]
        public GameObject introScreen;

        /// <summary>
        /// The Description screen is used to describe features in the scene.
        /// </summary>
        [Tooltip("Drag the 'description screen' under the items canvas game object")]
        public GameObject descriptionScreen;
    }
    public class IntroductionManager : MonoBehaviour
    {
        [SerializeField] private LearnVRSettings learnVRSettings;
        [SerializeField] private GeneralSettings generalSettings;
        [SerializeField] private RotationPivots rotationPivots;
        [SerializeField] private SpecificAnchor specificAnchor;
        [SerializeField] private CharacterSettings characterSettings;
        [SerializeField] private TargetedAudios targetedAudios;

        //[Tooltip("Drag the canvas game object which contains the 'description screen' game object.")]
        [SerializeField] private Canvas itemsCanvas;
        [SerializeField] private LocomotionMediator locomotion;
        [SerializeField] private AircraftSpawnManager aircraftSpawnManager;

        //We initialise this to subscribe to the event isLearnVRFinished, this helps us to start moving the user in the car 
        [SerializeField] private TutorialManager tutorialManager;

        [Header("Vehicles")]
        [SerializeField] private NavigateVehicle navigateVehicle;
        
        //We use this gameobject to lock the steering onto the car as it moves. We make the steering an independent gameobject  from the
        //car bacause we want to perform an easy tracking of the steering movement inside the corners.
        [Tooltip("Drag the steering pivot of your vehicle. This only works if you have enabled the above boolean doesVehicleHaveSteering")]
        [SerializeField] private GameObject steeringPivot;

        //this script contains an event which trigers whenever a user chooses an option to skip introduction and go straight to the cool stuff.
        [SerializeField] private LifecycleManager lifecycleManager;

        /// <summary>
        /// this variable is set by other scripts specifically LifecycleManager script with the purpose of 
        /// giving the player powers to skip all boring introduction. once set true, the player will be teleported
        /// to the entrance of the localizer shelter.
        /// </summary>
        [HideInInspector] public bool shouldSkipIntro;

        /// <summary>
        /// the purpose of this boolean relates to shouldSkipIntro boolean field. we want to make sure we execute
        /// the logic triggered by shouldSkipIntro in StageManager(Stages.stage6) only when the player is actually
        /// in the introduction phase, otherwise do nothing. 
        /// also its used in AircraftSpawnManager script, to start aircraft movement only after the introduction is finished.
        /// </summary>
        //[HideInInspector] public bool isIntroActive;

        /// <summary>
        /// Used to track the audio during introduction. 
        /// </summary>
        bool hasAudioEnded;

        /// <summary>
        /// used to keep player locked to the car seat as the car moves. 
        /// </summary>
        bool isPlayerInsideCar;

        /// <summary>
        /// Event which triggers when introduction finishes. This is access by event callback methods that must run after the introduction.
        /// </summary>
        [HideInInspector] public event Action isIntroFinished;

        private void OnEnable()
        {
            if (tutorialManager != null) tutorialManager.isLearnVRFinished += StartCarNavigation;
            if (lifecycleManager != null) lifecycleManager.shouldSkipIntro += () => shouldSkipIntro = true;
        }

        private void OnDisable()
        {
            if (tutorialManager != null) tutorialManager.isLearnVRFinished -= StartCarNavigation;
            if (lifecycleManager != null) lifecycleManager.shouldSkipIntro -= () => shouldSkipIntro = true;
        }

        void StartCarNavigation()
        {
            learnVRSettings.introScreen.SetActive(false);
            StageManager(Stages.stage1);

            //itemsCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            //StageManager(Stages.stage6);
        }

        private void Update()
        {
            if (shouldSkipIntro )
            {
                StageManager(Stages.stage6);

                //reset the variable to prevent trigger in the next frame.
                shouldSkipIntro = false;
            }
        }

        private void LateUpdate()
        {
            if (isPlayerInsideCar)
            {
                //lock player in car seat
                characterSettings.playerController.transform.SetPositionAndRotation(specificAnchor.passengerSeatAnchor.transform.position, specificAnchor.passengerSeatAnchor.transform.rotation);

            }
        }

        void StageManager(Enum stageToAccomplish)
        {
            //we use enums instead of strings, to prevent bugs that come from typos "stage1" vs "stge1"
            switch (stageToAccomplish)
            {
                case Stages.stage1:
                    //attach player into the vehicle.
                    //ManagePlayerTeleportation(specificAnchor.passengerSeatAnchor, () => {
                    //    isPlayerInsideCar = true;
                        //DescribeTaxiway();
                    //});
                    break;

                case Stages.stage2:
                    DescribeRunway();
                    break;

                case Stages.stage3:
                    DescribeAccessRoad();
                    break;
                  
                case Stages.stage4:
                    DescribeLocAntenna();
                    break;

                case Stages.stage5:
                    DescribeLocShelter();
                    break;

                case Stages.stage6:
                    StopAllCoroutines();

                    //detach the player from the car.
                    isPlayerInsideCar = false;
                    // teleport player to the entrance of the shelter
                    ManagePlayerTeleportation(
                        specificAnchor.finalAnchor,
                        () => {
                            // trigger the event that publishes the end of introduction.
                            isIntroFinished?.Invoke();
                        }
                    );
                    break;

            }
        }

        /// <summary>
        /// tracks the stop position of the vehicle as it moves during introduction.  
        /// </summary>
        Vector3 vehicleStopPosition;
        void DescribeTaxiway()
        {
            //ensure the boolean is false so that turning can take place.
            navigateVehicle.hasFinishedTurning = false;
            StartCoroutine(WaitForAudio(targetedAudios.taxiway));
            navigateVehicle.TurnVehicle(rotationPivots.apron5Pivot);
            StartCoroutine(WaitForAnyCondition(
                conditionMethod: () => navigateVehicle.hasFinishedTurning,
                actionMethod: () =>
                {
                    navigateVehicle.hasFinishedTurning = false;
                    vehicleStopPosition = new Vector3(navigateVehicle.transform.position.x, navigateVehicle.transform.position.y, 278);
                    StartCoroutine(navigateVehicle.MoveVehicleForward(vehicleStopPosition, rotationPivots.taxiRunwayPivot));
                    StartCoroutine(WaitForAnyCondition(
                        () => navigateVehicle.hasVehicleReached,
                        () => StageManager(Stages.stage2)
                    ));
                }
            ));
            
        }

        void DescribeRunway()
        {
            vehicleStopPosition = new Vector3(navigateVehicle.transform.position.x, navigateVehicle.transform.position.y, 100);
            ManagePlayerLocomotionDuringIntro(vehicleStopPosition, targetedAudios.runway,()=> StageManager(Stages.stage3), rotationPivots.runwayAccessRoadPivot);
        }

        void DescribeAccessRoad()
        {
            vehicleStopPosition = new Vector3(navigateVehicle.transform.position.x, navigateVehicle.transform.position.y, 284);
            ManagePlayerLocomotionDuringIntro(vehicleStopPosition, targetedAudios.accessRoad, () => StageManager(Stages.stage4), rotationPivots.accessRdRwyPivot);
        }

        void DescribeLocAntenna()
        {
            navigateVehicle.hasFinishedTurning = false;
            vehicleStopPosition = new Vector3(1399, navigateVehicle.transform.position.y, navigateVehicle.transform.position.z);
            StartCoroutine(navigateVehicle.MoveVehicleForward(vehicleStopPosition));
            StartCoroutine(WaitForAudio(targetedAudios.locAntenna));
            StartCoroutine(WaitForAnyCondition(
                () => navigateVehicle.hasVehicleReached,
                () => { 
                    navigateVehicle.TurnVehicle(rotationPivots.locAntennaPivot);
                }
            ));
            StartCoroutine(WaitForAnyCondition(
               conditionMethod: () => navigateVehicle.hasFinishedTurning,
               actionMethod: () => StageManager(Stages.stage5)
           ));
        }
        void DescribeLocShelter()
        {
            navigateVehicle.hasFinishedTurning = false;
            navigateVehicle.TurnVehicle(rotationPivots.locShelterPivot);
            StartCoroutine(WaitForAnyCondition(
                conditionMethod: () => navigateVehicle.hasFinishedTurning,
                actionMethod: () =>
                {
                    vehicleStopPosition = new Vector3(1348, navigateVehicle.transform.position.y, navigateVehicle.transform.position.z);
                    ManagePlayerLocomotionDuringIntro(vehicleStopPosition, targetedAudios.locShelter,
                        () =>
                        {
                            StartCoroutine(WaitForAudio(targetedAudios.finalStatement));
                            StartCoroutine(WaitForAnyCondition(
                                conditionMethod: () => hasAudioEnded,
                                actionMethod: () => StageManager(Stages.stage6)
                            ));
                           
                        }
                    );
                }));
        }


        /// <summary>
        /// This method is used to wait for any event controlled by a booolean, any other actions performed after the boolean is true will be passed to the actionMethod 
        /// parameter when wrapped inside a method without parameters and which returns no value.
        /// </summary>
        /// <param name="conditionMethod">The method which controls the event, it must return a bool (true) when event is accompliseh</param>
        /// <param name="actionMethod"> (Optional) The method that contains logic that runs after the control boolean becomes true. This method must not have parameters and must 
        /// not return any value</param>
        /// <returns></returns>
        IEnumerator WaitForAnyCondition(Func<bool> conditionMethod1, Func<bool> conditionMethod2, Action actionMethod = null)
        {
            while (!conditionMethod1() || !conditionMethod2())
            {
                yield return null;
            }

            actionMethod?.Invoke();
        }

        IEnumerator WaitForAnyCondition(Func<bool> conditionMethod, Action actionMethod = null)
        {
            while (!conditionMethod())
            {
                yield return null;
            }

            actionMethod?.Invoke();
        }


        IEnumerator WaitForAudio(AudioSource targetAudio)
        {
            hasAudioEnded = false;
            generalSettings.soundController.PlaySound(targetAudio);
            yield return new WaitWhile(() => targetAudio.isPlaying);

            hasAudioEnded = true;
        }

        /// <summary>
        /// Ensures player movement from current position to finalVehiclePosition. the method has an optional parameter targetPivot.
        /// If at the end of the movement there is no turning, do not assign a value to the parameter.
        /// </summary>
        /// <param name="finalVehiclePosition">The target final position</param>
        /// <param name="targetAudio">Audios you want to play along as the vehicle moves.</param>
        /// <param name="targetPivot">An optional parameter, the pivot which will help the vehicle turn</param>
        /// <param name="nextStage">The enum value that triggers the next stage.</param>
        private void ManagePlayerLocomotionDuringIntro(Vector3 finalVehiclePosition, AudioSource targetAudio, Action actionAfterLocomotion, BasePivot targetPivot = null)
        {
            StartCoroutine(navigateVehicle.MoveVehicleForward(finalVehiclePosition, targetPivot));
            StartCoroutine(WaitForAudio(targetAudio));
            StartCoroutine(WaitForAnyCondition(
                () => navigateVehicle.hasVehicleReached,
                () => hasAudioEnded,
                () => actionAfterLocomotion()
            ));
        }

        private void ManagePlayerTeleportation(TeleportationAnchor targetAnchor, Action ExecuteLogicAfterTeleport)
        {
            generalSettings.teleportPlayer.hasTeleported = false;
            generalSettings.teleportPlayer.RequestToTeleportToAnchor(targetAnchor);

            StartCoroutine(WaitForAnyCondition(
                () => generalSettings.teleportPlayer.hasTeleported,
                () => ExecuteLogicAfterTeleport() 
            ));
        }
    }

}

    