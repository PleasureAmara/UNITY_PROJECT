using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

namespace localizer.product.player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;
        [SerializeField] private float rotationSpeed;

        private Vector3 camera3DMovement;
        private float cameraRotate;

        [SerializeField] private InputActionAsset inputActionAsset;
        private InputActionMap inputActionMap;
        private InputAction movePlayer;
        private InputAction rotatePlayer;

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

        private void MovePlayer_canceled(InputAction.CallbackContext obj)
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

