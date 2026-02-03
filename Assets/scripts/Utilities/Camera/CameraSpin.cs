using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSpin : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActionAsset;
    private InputActionMap cameraActionMap;
    private InputAction cameraRotateAction;
    private InputAction cameraMoveAction;
    private InputAction cameraSidesMoveAction;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private float moveSpeed;

    private void Awake()
    {
        cameraActionMap = inputActionAsset.FindActionMap("Gameplay");
        cameraRotateAction = cameraActionMap.FindAction("SpinCamera");
        cameraMoveAction = cameraActionMap.FindAction("MoveCamera");
        cameraSidesMoveAction = cameraActionMap.FindAction("sideCameraMove");

        //cameraRotateAction.performed += RotateCamera;
    }

    void OnEnable()
    {
        cameraRotateAction.Enable();
        cameraMoveAction.Enable();
        cameraSidesMoveAction.Enable();
    }

    private void OnDisable()
    {
        cameraRotateAction.Disable();
        cameraMoveAction.Disable();
        cameraSidesMoveAction.Disable();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
        MoveCamera();
    }

    void RotateCamera()
    {
        transform.Rotate(Vector3.up * cameraRotateAction.ReadValue<float>() * rotationSpeed * Time.deltaTime);
    }

    void MoveCamera()
    {
        float moveInput = cameraMoveAction.ReadValue<float>();
        float sideMotionInput = cameraSidesMoveAction.ReadValue<float>();
        transform.Translate(Vector3.forward * moveInput * moveSpeed * Time.deltaTime);
        transform.Translate(Vector3.left * sideMotionInput * moveSpeed * Time.deltaTime);
    }
    //void SidesCameraMove()
    //{

    //}
}
