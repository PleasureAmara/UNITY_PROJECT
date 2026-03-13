using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;

using localizer.product.environment;
using localizer.core.enums;
using System;

namespace localizer.product.player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private float rotationSpeed;

        private CharacterController playerController;

        [SerializeField] private InputActionAsset inputActionAsset;
        private InputActionMap inputActionMap;
        private InputAction movePlayer;
        private InputAction rotatePlayer;

        // Add missing field for camera3DMovement
        private Vector3 camera3DMovement = Vector3.zero;
        // Add missing field for cameraRotate
        private float cameraRotate = 0.0f;

        private void Awake()
        {
            inputActionMap = inputActionAsset.FindActionMap("Gameplay");
            movePlayer = inputActionMap.FindAction("MoveCamera");
            //movePlayerSides = inputActionMap.FindAction("sideCameraMove");
            rotatePlayer = inputActionMap.FindAction("SpinCamera");

            movePlayer.performed += context => camera3DMovement = context.ReadValue<Vector3>();
            movePlayer.canceled += context => camera3DMovement = Vector3.zero;

            rotatePlayer.performed += context => cameraRotate = context.ReadValue<float>();
            rotatePlayer.canceled += context => cameraRotate = 0.0f;
        }

        void DisablePlayerHands()
        {
            throw new System.NotImplementedException();
        }

        private void OnEnable()
        {
            movePlayer.Enable();
            rotatePlayer.Enable();
        }

        private void OnDisable()
        {
            movePlayer.Disable();
            rotatePlayer.Disable();
        }

        void Start()
        {
            // Removed the StageManager line as it's not defined in the provided context
        }

        void MoveCamera()
        {
            float forwardMovement = camera3DMovement.x;
            float sideMovement = camera3DMovement.y;
            float upwardMovement = camera3DMovement.z;

            transform.Translate(Vector3.forward * sideMovement * movementSpeed * Time.deltaTime);
            transform.Translate(Vector3.right * forwardMovement * movementSpeed * Time.deltaTime);
            transform.Translate(Vector3.up * upwardMovement * movementSpeed * Time.deltaTime);

        }

        void RotateCamera()
        {
            transform.Rotate(Vector3.up * cameraRotate * rotationSpeed * Time.deltaTime);
        }

        void Update()
        {
            MoveCamera();
            RotateCamera();
        }

    }
}

