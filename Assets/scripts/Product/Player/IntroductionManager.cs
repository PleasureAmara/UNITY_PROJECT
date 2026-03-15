using localizer.core.enums;
using localizer.product.descriptions;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

namespace localizer.product.player
{
    [Serializable]
    public class GeneralSettings
    {
        public TeleportPlayer teleportPlayer;
        public SoundController soundController;
        public TeleportationProvider teleportationProvider;
    }

    [Serializable]
    public class SpecificAnchor
    {
        [Header("Introduction anchor")]
        public TeleportationAnchor introAnchor;

        [Header("Final position anchor")]
        public TeleportationAnchor finalAnchor;

    }

    [Serializable]
    public class TargetedAudios
    {
        public AudioSource antennaDescriptionAudio;
        public AudioSource shelterDescriptionAudio;
    }

    [Serializable]
    public class CharacterMovementSettings
    {
        [Tooltip("Attach the XR ORIGIN gameobject")]
        public CharacterController playerController;

        [Tooltip("The speed with which the character is to move forward along the runway")]
        public float forwardMovementSpeed;

        [Tooltip("The speed with which player is moving sideways from loc antennas to the shelter")]
        public float sidesMovementSpeed;
    }
    public class IntroductionManager : MonoBehaviour
    {

        [SerializeField] private GeneralSettings generalComponents;
        [SerializeField] private SpecificAnchor specificAnchor;
        [SerializeField] private CharacterMovementSettings characterMovementSettings;
        [SerializeField] private TargetedAudios targetedAudios;

        [Header("Introduction audios")]
        [Tooltip("Add audio sources in the order you want them to play")]
        [SerializeField] private AudioSource[] audioSources;

        [Tooltip("Drag the show description component attached to this gameobject.")]
        [SerializeField] private ShowDescription showDescription;

        //these variables control how far the player is moved forward -> z-value and left -> x-value
        private float finalXValue = 1317.0f;
        private float finalZValue = 350.0f;
    

        void Start()
        {

            StageManager(Stages.stage0);
        }
        void DisablePlayerHands()
        {
            //transform.Find("Locomotion").gameObject.SetActive(false);
            characterMovementSettings.playerController.gameObject.transform.Find("Camera Offset/Right Controller").gameObject.SetActive(false);
        }

        void EnablePlayerHands()
        {
            //transform.Find("Locomotion").gameObject.SetActive(true);
            characterMovementSettings.playerController.gameObject.transform.Find("Camera Offset/Right Controller").gameObject.SetActive(true);
        }


        void StageManager(Enum stageAccomplished)
        {
            //we use enums instead of strings, to prevent bugs that come from typos "stage1" vs "stge1"
            switch (stageAccomplished)
            {
                case Stages.stage0:
                    DisablePlayerHands();

                    StartCoroutine(MovePlayerToAntennaCoroutine());
                    StartCoroutine(RunIntroAudioCoroutine());
                    break;

                case Stages.stage1:
                    StopAllCoroutines();
                    StartCoroutine(RunAntennaAudioCoroutine());
                    break;

                case Stages.stage2:
                    StopAllCoroutines();
                    StartCoroutine(MovePlayerToLocShelter());
                    StartCoroutine(RunShelterAudioCoroutine());
                    break;

                case Stages.stage3:
                    StopAllCoroutines();
                    PositionPlayerToFinalAnchor();
                    showDescription.RenderScreen();
                    EnablePlayerHands();
                    break;

            }
        }

        IEnumerator RunIntroAudioCoroutine()
        {
            if (audioSources == null) yield return null;

            foreach (var source in audioSources)
            {
                generalComponents.soundController.PlaySound(source);
                yield return new WaitWhile(() => source.isPlaying);
            }
            
            
            
        }
        IEnumerator MovePlayerToAntennaCoroutine()
        {
            generalComponents.teleportPlayer.TeleportToAnchor(specificAnchor.introAnchor, generalComponents.teleportationProvider);

            while (characterMovementSettings.playerController.gameObject.transform.position.z < finalZValue)
            {
                characterMovementSettings.playerController.Move(characterMovementSettings.forwardMovementSpeed * Time.deltaTime *Vector3.forward );
                yield return null;
            }
            //To prevent overshooting due to player movement along limit, we can snap their final position.
            //Vector3 finalPosition = transform.position;
            //finalPosition.z = finalZValue;
            //transform.position = finalPosition;

            StageManager(Stages.stage1);
        }

        IEnumerator RunAntennaAudioCoroutine()
        {
            generalComponents.soundController.PlaySound(targetedAudios.antennaDescriptionAudio);
            yield return new WaitWhile(() => targetedAudios.antennaDescriptionAudio.isPlaying);
            StageManager(Stages.stage2);
        }

        IEnumerator MovePlayerToLocShelter()
        {
            while (characterMovementSettings.playerController.gameObject.transform.position.x > finalXValue)
            {
                characterMovementSettings.playerController.Move( characterMovementSettings.sidesMovementSpeed * Time.deltaTime *Vector3.left );
                yield return null;
            }

            StageManager(Stages.stage3);
        }

        IEnumerator RunShelterAudioCoroutine()
        {
            generalComponents.soundController.PlaySound(targetedAudios.shelterDescriptionAudio);
            yield return new WaitWhile(() => targetedAudios.shelterDescriptionAudio.isPlaying);
        }

        void PositionPlayerToFinalAnchor()
        {
            generalComponents.teleportPlayer.TeleportToAnchor(specificAnchor.finalAnchor, generalComponents.teleportationProvider);

        }
    }

}

    