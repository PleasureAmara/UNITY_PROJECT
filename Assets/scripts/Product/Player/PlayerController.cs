using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
//using UnityEngine.XR.Interaction.Toolkit;
//using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
//using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

using localizer.product.environment;
using localizer.core.enums;
using System;

namespace localizer.product.player
{
    public class PlayerController : MonoBehaviour
    {
        //public float zValue;
        [SerializeField]private float rwyMoveSpeed = 100.0f;
        [SerializeField] private float sidesMoveSpeed = 30.0f;
        public float moveDelay =0.1f;
        [SerializeField] private float rotationUnitAngle = 90.0f;
        private float rotationDelay = 0.2f;

        private CharacterController playerController;

        //[SerializeField]private Transform locAntennaTransform;
        //[SerializeField] private Transform locShelterTransform;

        
        //We create variables for storing various coroutines, this is because you cant stop an active coroutine by calling it directly since Unity engine
        //will create a new instance of the coroutine and stop that instead of stop the active coroutine.
        //To solve this, we store the coroutine when starting it, this way we stop the actively runnig coroutine.
        Coroutine MovePlayerToAntennaCoroutine;
        Coroutine MovePlayerToLocShelterCoroutine;
        Coroutine MovePlayerToLocDoorCoroutine;
        Coroutine RotateTowardsLocDoorCoroutine;



        void Start()
        {
            playerController = GetComponent<CharacterController>();
            StageManager(Stages.stage0 );
        }

        void StageManager(Enum stageAccomplished)
        {
            //we use enums instead of strings, to prevent bugs that come from typos "stage1" vs "stge1"
            switch (stageAccomplished)
            {
                case Stages.stage0:
                    DisablePlayerHands();
                    MovePlayerToAntennaCoroutine = StartCoroutine(MovePlayerToAntenna());
                    break;

                case Stages.stage1:
                    if (MovePlayerToAntennaCoroutine == null) return;
                    StopCoroutine(MovePlayerToAntennaCoroutine);
                    MovePlayerToLocShelterCoroutine = StartCoroutine(MovePlayerToLocShelter());
                    break;

                case Stages.stage2:
                    if (MovePlayerToLocShelterCoroutine == null) return;
                    StopCoroutine(MovePlayerToLocShelterCoroutine);
                    MovePlayerToLocDoorCoroutine = StartCoroutine(MovePlayerToLocDoor());
                    break;

                case Stages.stage3:
                    if (MovePlayerToLocDoorCoroutine == null) return;
                    StopCoroutine(MovePlayerToLocDoorCoroutine);
                    RotateTowardsLocDoorCoroutine = StartCoroutine(RotateTowardsLocDoor());
                    break;

                case Stages.stage4:
                    if (RotateTowardsLocDoorCoroutine == null) return ;
                    StopCoroutine(RotateTowardsLocDoorCoroutine);
                    EnablePlayerHands();


                    break;
            }
        }

        void DisablePlayerHands()
        {
            transform.Find("Locomotion").gameObject.SetActive(false);
            //transform.Find("Camera Offset/Right Controller").gameObject.SetActive(false);
        }

        void EnablePlayerHands()
        {
            transform.Find("Locomotion").gameObject.SetActive(true);
            //transform.Find("Camera Offset/Right Controller").gameObject.SetActive(true);
        }
        IEnumerator MovePlayerToAntenna()
        {
            while (transform.position.z < 350)
            {
                playerController.Move(Vector3.forward * rwyMoveSpeed * Time.deltaTime);

                //waiting for frames is more efficient than waiting for seconds.
                yield return null;
            }
            //To prevent overshooting due to player movement along limit, we can snap their final position.
            Vector3 finalPosition = transform.position;
            finalPosition.z = 350;
            transform.position = finalPosition;

            //calls the method StageManager with the stage it belongs to as a parameter.
            StageManager(Stages.stage1);
        }

        IEnumerator MovePlayerToLocShelter()
        {
            while (transform.position.x > 1307)
            {
                playerController.Move(Vector3.left * sidesMoveSpeed * Time.deltaTime);
                yield return null;
            }

            Vector3 finalPosition = transform.position;
            finalPosition.x = 1307;
            transform.position = finalPosition;

            StageManager(Stages.stage2);
        }

        IEnumerator MovePlayerToLocDoor()
        {
            while (transform.position.z < 360)
            {
                playerController.Move(Vector3.forward * sidesMoveSpeed * Time.deltaTime);
                yield return null;
            }

            Vector3 finalPosition = transform.position;
            finalPosition.z = 360;
            transform.position = finalPosition;

            StageManager(Stages.stage3);
        }

        IEnumerator RotateTowardsLocDoor()
        {
            float duration = 1.0f; // How many seconds the turn should take
            float elapsed = 0.0f;

            Quaternion startRotation = transform.rotation;
            // Calculate the target: Current rotation + 90 degrees on Y axis
            Quaternion targetRotation = transform.rotation * Quaternion.Euler(0, 90, 0);

            while (elapsed < duration)
            {
                // Calculate how far we are through the duration (0.0 to 1.0)
                elapsed += Time.deltaTime;
                float percent = elapsed / duration;

                // Smoothly interpolate between start and target
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, percent);

                yield return null; // Wait for the next frame
            }

            // Ensure we land exactly on the rotation
            transform.rotation = targetRotation;

            StageManager(Stages.stage4);
        }

    }
}

