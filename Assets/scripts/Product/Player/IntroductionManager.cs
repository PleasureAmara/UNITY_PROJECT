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
        public BasePivot taxiRunwayPivot;
        public BasePivot runwayAccessRoadPivot;
        public BasePivot accessRdRwyPivot;
        public BasePivot locAntennaPivot;
        public BasePivot locAntennaToShelterPivot;
        public BasePivot locShelterPivot;
    }


    [Serializable]
    public class SpecificAnchor
    {
        [Header("Introduction anchor")]
        public TeleportationAnchor introAnchor;

        [Header("taxiway position anchor")]
        public TeleportationAnchor taxiwayAnchor;

        [Header("runway position anchor")]
        public TeleportationAnchor runwayAnchor;

        [Header("access road position anchor")]
        public TeleportationAnchor accessRoadAnchor;

        [Header("localizer antenna position anchor")]
        public TeleportationAnchor locAntennaAnchor;

        [Header("Final position anchor")]
        public TeleportationAnchor finalAnchor;

        
    }

    [Serializable]
    public class TargetedAudios
    {
        public AudioSource taxiway;
        public AudioSource runway;
        public AudioSource accessRoad;
        public AudioSource locAntenna;
        public AudioSource locShelter;
        //public AudioSource antennaDescriptionAudio;
        //public AudioSource shelterDescriptionAudio;
    }

    [Serializable]
    public class CharacterSettings
    {
        [Tooltip("Attach the XR ORIGIN gameobject")]
        public CharacterController playerController;

    }

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

        [Header("Vehicles")]
        [SerializeField] private NavigateVehicle navigateVehicle;

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
        /// </summary>
        private bool isIntroActive;

        /// <summary>
        /// Used to track the audio during introduction. 
        /// </summary>
        bool hasAudioEnded;

        void Start()
        {
            generalSettings.showDescription.descriptionScreen.SetActive(false);
            learnVRSettings.introScreen.SetActive(false);
            //itemsCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            StageManager(Stages.stage0);

        }

        private void Update()
        {
            if (shouldSkipIntro && isIntroActive)
            {
                StageManager(Stages.stage6);

                //reset the variable to prevent trigger in the next frame.
                shouldSkipIntro = false;
            }
        }

        void DisablePlayerHands()
        {
            //transform.Find("Locomotion").gameObject.SetActive(false);
            characterSettings.playerController.gameObject.transform.Find("Camera Offset/Right Controller").gameObject.SetActive(false);
        }

        void EnablePlayerHands()
        {
            //transform.Find("Locomotion").gameObject.SetActive(true);
            characterSettings.playerController.gameObject.transform.Find("Camera Offset/Right Controller").gameObject.SetActive(true);
        }


        void StageManager(Enum stageToAccomplish)
        {
            //we use enums instead of strings, to prevent bugs that come from typos "stage1" vs "stge1"
            switch (stageToAccomplish)
            {
                case Stages.stage0:
                    isIntroActive = true;
                    NavigateIntroductionMenu();
                    break;

                case Stages.stage1:
                    locomotion.gameObject.SetActive(false);

                    //attach player into the vehicle.
                    characterSettings.playerController.transform.SetParent(navigateVehicle.transform, worldPositionStays: false);
                    characterSettings.playerController.transform.localPosition = new Vector3(-2.63f, 0.79f, 5.39f);
                    characterSettings.playerController.transform.rotation = Quaternion.Euler(0, 0, 0);
                    DescribeTaxiway();
                    break;

                case Stages.stage2:
                    DescribeRunway();
                    break;

                case Stages.stage3:
                    DescribeAccessRoad();
                    break;
                  
                case Stages.stage4:
                    //hasAudioEnded = false;
                    //locomotion.gameObject.SetActive(true);
                    //characterSettings.playerController.transform.SetParent(null, true);
                    DescribeLocAntenna();
                    break;

                //we set hasAudioEnded = false in case statements in stage4 and 5 because of the teleportation. originally the variable is set inside the WaitForAudio() methods at their start.
                //the methods that control stage4 and stage5 have 2 coroutines i.e the one that teleports the player and the one
                //that waits for everything to end before calling next stage. however teleportation doesnt happen rightaway and
                //since the methods are non blocking, the compiler will trigger the second coroutine before teleportation ends. the 
                //variable tracked in the second coroutine hasAudioEnded is managed by the 1st coroutine, thus it can trigger next
                //stage before the current one ends.
                //setting the hasAudioEnded in the case statement ensures the next stage never triggers until the first coroutine finishes.
                case Stages.stage5:
                    //hasAudioEnded = false;
                    DescribeLocShelter();
                    //isIntroActive = false;
                    break;

                //this stage 6 is only called if the boolean variable shouldSkipIntro is activated by the user
                //during the introduction tutorials.
                case Stages.stage6:
                    StopAllCoroutines();
                    Debug.Log("At last, we completed the battle.");
                    //ensure the player isnt a child of any gameobject
                    locomotion.gameObject.SetActive(true);
                    characterSettings.playerController.transform.SetParent(null, true);

                    // teleport player to the entrance of the shelter
                    ManagePlayerTeleportation(
                        specificAnchor.finalAnchor,
                        () => { }
                    );
                    break;

            }
        }

        void NavigateIntroductionMenu()
        {
            //we set the boolean to false to track the next stage of teleportation.
            learnVRSettings.learnVRControllers.isIntroFinished = false;
            ManagePlayerTeleportation(specificAnchor.introAnchor, 
                () => {
                    learnVRSettings.introScreen.SetActive(true);

                    //Start the learn VR screens.
                    learnVRSettings.learnVRControllers.SetUpInitialState();

                    StartCoroutine(WaitForAnyCondition(
                        conditionMethod: () => learnVRSettings.learnVRControllers.isIntroFinished,
                        actionMethod: () => {
                            learnVRSettings.introScreen.SetActive(false);

                            StageManager(Stages.stage1);
                        }
                    ));
                });

        }
        /// <summary>
        /// tracks the stop position of the vehicle as it moves during introduction.  
        /// </summary>
        Vector3 vehicleStopPosition;
        void DescribeTaxiway()
        {
            vehicleStopPosition = new Vector3(navigateVehicle.transform.position.x, navigateVehicle.transform.position.y, 277);
            ManagePlayerLocomotionDuringIntro(vehicleStopPosition, targetedAudios.taxiway, Stages.stage2, rotationPivots.taxiRunwayPivot);
        }

        void DescribeRunway()
        {
            vehicleStopPosition = new Vector3(navigateVehicle.transform.position.x, navigateVehicle.transform.position.y, -355);
            ManagePlayerLocomotionDuringIntro(vehicleStopPosition, targetedAudios.runway, Stages.stage3, rotationPivots.runwayAccessRoadPivot);
        }

        void DescribeAccessRoad()
        {
            vehicleStopPosition = new Vector3(navigateVehicle.transform.position.x, navigateVehicle.transform.position.y, 292);
            ManagePlayerLocomotionDuringIntro(vehicleStopPosition, targetedAudios.accessRoad, Stages.stage4, rotationPivots.accessRdRwyPivot);
        }

        void DescribeLocAntenna()
        {
            vehicleStopPosition = new Vector3(1374, navigateVehicle.transform.position.y, navigateVehicle.transform.position.z);
            ManagePlayerLocomotionDuringIntro(vehicleStopPosition, targetedAudios.locAntenna, Stages.stage5);
            //ManagePlayerTeleportation(
            //    specificAnchor.locAntennaAnchor,
            //    () => StartCoroutine(WaitForAudio(targetedAudios.locAntenna))
            //);
            //StartCoroutine(WaitForAnyCondition(
            //    () => hasAudioEnded,
            //    () => StageManager(Stages.stage5)
            //    ));
        }
        void DescribeLocShelter()
        {
            vehicleStopPosition = new Vector3(1313, navigateVehicle.transform.position.y, navigateVehicle.transform.position.z);
            //navigateVehicle.hasFinishedTurning = false;
            //navigateVehicle.TurnVehicle(rotationPivots.locAntennaToShelterPivot);
            //StartCoroutine(WaitForAnyCondition(
            //    conditionMethod: () => navigateVehicle.hasFinishedTurning,
            //    actionMethod: ()=>
            //    {
            //        ManagePlayerLocomotionDuringIntro(vehicleStopPosition, targetedAudios.locShelter, Stages.stage6, rotationPivots.locShelterPivot);
            //    }));
            ManagePlayerLocomotionDuringIntro(vehicleStopPosition, targetedAudios.locShelter, Stages.stage6);
            //ManagePlayerTeleportation(
            //    specificAnchor.finalAnchor,
            //    () => StartCoroutine(WaitForAudio(targetedAudios.locShelter))
            //);
            //StartCoroutine(WaitForAnyCondition(
            //    () => hasAudioEnded,
            //    () => StageManager(Stages.stage6)
            //    ));
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
        private void ManagePlayerLocomotionDuringIntro(Vector3 finalVehiclePosition, AudioSource targetAudio, Enum nextStage, BasePivot targetPivot = null)
        {
            StartCoroutine(navigateVehicle.MoveVehicleForward(finalVehiclePosition, targetPivot));
            StartCoroutine(WaitForAudio(targetAudio));
            StartCoroutine(WaitForAnyCondition(
                () => navigateVehicle.hasVehicleReached,
                () => hasAudioEnded,
                () => StageManager(nextStage)
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

    