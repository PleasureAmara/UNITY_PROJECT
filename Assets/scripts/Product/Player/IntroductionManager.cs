using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

using localizer.core.enums;
using localizer.product.descriptions;

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
        public AudioSource taxiway_1;
        public AudioSource taxiway_2;
        public AudioSource runway;
        public AudioSource accessRoad;
        public AudioSource locAntenna_1;
        public AudioSource locAntenna_2;
        public AudioSource locShelter_1;
        public AudioSource locShelter_2;
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
        [SerializeField] private SpecificAnchor specificAnchor;
        [SerializeField] private CharacterSettings characterSettings;
        [SerializeField] private TargetedAudios targetedAudios;

        //[Tooltip("Drag the canvas game object which contains the 'description screen' game object.")]
        [SerializeField] private Canvas itemsCanvas;

        void Start()
        {
            //generalSettings.showDescription.descriptionScreen.SetActive(false);
            //learnVRSettings.introScreen.SetActive(false);
            //itemsCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            //StageManager(Stages.stage0);

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
                    NavigateIntroductionMenu();
                    break;

                case Stages.stage1:
                    StopAllCoroutines();
                    DescribeTaxiway();
                    break;

                case Stages.stage2:
                    StopAllCoroutines();
                    DescribeRunway();   
                    break;

                case Stages.stage3:
                    StopAllCoroutines();
                    DescribeAccessRoad();
                    break;

                case Stages.stage4:
                    StopAllCoroutines();
                    DescribeLocAntenna();
                    break;
                case Stages.stage5:
                    StopAllCoroutines();
                    PositionPlayerToFinalAnchor();
                    break;
                case Stages.stage6:
                    StopAllCoroutines();
                    //generalSettings.showDescription.RenderScreen();
                    break;

            }
        }

        void NavigateIntroductionMenu()
        {
            //we set the boolean to false to track the next stage of teleportation.
            learnVRSettings.learnVRControllers.isIntroFinished = false;
            ManagePlayerTeleportation(ActionsAfterInitialTeleportation, specificAnchor.introAnchor);
        
        }

        void ActionsAfterInitialTeleportation()
        {
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
        }

        void DescribeTaxiway()
        {
            ManagePlayerTeleportation( 
                () => {
                    AudioSource[] taxiwayAudios = new AudioSource[] { targetedAudios.taxiway_1, targetedAudios.taxiway_2 };
                    StartCoroutine(WaitForAudio(targetAudios: taxiwayAudios, nextStage: Stages.stage2));
                },
                specificAnchor.taxiwayAnchor);
        }

        void DescribeRunway()
        {
            ManagePlayerTeleportation(
                () => {
                    StartCoroutine(WaitForAudio(targetAudio: targetedAudios.runway, nextStage: Stages.stage3));
                }, specificAnchor.runwayAnchor);
        }

        void DescribeAccessRoad()
        {
            ManagePlayerTeleportation(
                ()=> {
                    StartCoroutine(WaitForAudio(targetAudio: targetedAudios.accessRoad, nextStage: Stages.stage4));
                }, specificAnchor.accessRoadAnchor);
        }

        void DescribeLocAntenna()
        {
            ManagePlayerTeleportation(() =>
            {
                AudioSource[] locAntennaAudios = new AudioSource[] { targetedAudios.locAntenna_1, targetedAudios.locAntenna_2 };
                StartCoroutine(WaitForAudio(targetAudios: locAntennaAudios, nextStage: Stages.stage5));
            }, specificAnchor.locAntennaAnchor);
        }
        void PositionPlayerToFinalAnchor()
        {
            ManagePlayerTeleportation(
                ()=> {
                    AudioSource[] locshelterAudios = new AudioSource[] { targetedAudios.locShelter_1, targetedAudios.locShelter_2 };
                    StartCoroutine(WaitForAudio(targetAudios: locshelterAudios, nextStage: Stages.stage6));
                }, specificAnchor.finalAnchor);
        }


        /// <summary>
        /// This method is used to wait for any event controlled by a booolean, any other actions performed after the boolean is true will be passed to the actionMethod 
        /// parameter when wrapped inside a method without parameters and which returns no value.
        /// </summary>
        /// <param name="conditionMethod">The method which controls the event, it must return a bool (true) when event is accompliseh</param>
        /// <param name="actionMethod"> (Optional) The method that contains logic that runs after the control boolean becomes true. This method must not have parameters and must 
        /// not return any value</param>
        /// <returns></returns>
        IEnumerator WaitForAnyCondition(Func<bool> conditionMethod, Action actionMethod = null)
        {
            while (!conditionMethod())
            {
                yield return null;
            }

            actionMethod?.Invoke();
        }


        IEnumerator WaitForAudio(AudioSource[] targetAudios, Enum nextStage)
        {
            foreach(AudioSource audioSource in targetAudios)
            {
                generalSettings.soundController.PlaySound(audioSource);
                yield return new WaitWhile(() => audioSource.isPlaying);
            }

            StageManager(nextStage);
        }

        IEnumerator WaitForAudio(AudioSource targetAudio, Enum nextStage = null)
        {
            generalSettings.soundController.PlaySound(targetAudio);
            yield return new WaitWhile(() => targetAudio.isPlaying);

            if (nextStage != null) StageManager(nextStage);
        }

        private void ManagePlayerTeleportation(Action ExecuteLogicAfterTeleport, TeleportationAnchor targetAnchor)
        {
            generalSettings.teleportPlayer.hasTeleported = false;
            generalSettings.teleportPlayer.RequestToTeleportToAnchor(targetAnchor);

            StartCoroutine(WaitForAnyCondition(
                () => generalSettings.teleportPlayer.hasTeleported,
                () => {
                    ExecuteLogicAfterTeleport();
                }
            ));
        }
    }

}

    