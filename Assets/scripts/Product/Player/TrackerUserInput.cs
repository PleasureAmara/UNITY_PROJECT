using UnityEngine;
using UnityEngine.InputSystem;

public class TrackerUserInput : MonoBehaviour
{
    [SerializeField] private InputActionAsset xriActionAsset;
    InputActionMap leftHandMap;
    InputActionMap rightHandMap;
    //InputActionMap left;
    //InputActionMap right;
    InputActionMap leftLocomotion;
    InputActionMap rightLocomotion;

    [HideInInspector] public string pressedKey;

    private void Awake()
    {
        leftHandMap = xriActionAsset.FindActionMap("XRI Left Interaction");
        rightHandMap = xriActionAsset.FindActionMap("XRI Right Interaction");
        //left = xriActionAsset.FindActionMap("XRI Left");
        //right = xriActionAsset.FindActionMap("XRI Right");
        leftLocomotion = xriActionAsset.FindActionMap("XRI Left Locomotion");
        rightLocomotion = xriActionAsset.FindActionMap("XRI Right Locomotion");
    }
    private void OnEnable()
    {
        MapActionMapToEventMethod(leftHandMap);
        MapActionMapToEventMethod(rightHandMap);
        //MapActionMapToEventMethod(left);
        //MapActionMapToEventMethod(right);
        MapActionMapToEventMethod(leftLocomotion);
        MapActionMapToEventMethod(rightLocomotion);

    }

    private void OnDisable()
    {
        UnMapActionMapToEventMethod(leftHandMap);
        UnMapActionMapToEventMethod(rightHandMap);
        //UnMapActionMapToEventMethod(left);
        //UnMapActionMapToEventMethod(right);
        UnMapActionMapToEventMethod(leftLocomotion);
        UnMapActionMapToEventMethod(rightLocomotion);

    }

    void MapActionMapToEventMethod(InputActionMap actionMap)
    {

        foreach (var action in actionMap.actions)
        {
            action.Enable();
            //Debug.Log($"{action.name} : {action.type}");
            if (action.type == InputActionType.Button)
            {
                action.performed += ManageCapturedAction;
                Debug.Log($"{action.name} : {action.type}");
            }

            if (action.type ==InputActionType.Value) // && action.name == "Move")
            {
                action.started += ManageCapturedAction;
                action.performed += ManageCapturedAction;
                Debug.Log($"{action.name} : {action.type}");
            }
        }
    }

    void UnMapActionMapToEventMethod(InputActionMap actionMap)
    {
        foreach (var action in actionMap.actions)
        {
            if (action.type == InputActionType.Value)
            {
                action.performed -= ManageCapturedAction;
            }

            if (action.type == InputActionType.Value) 
            {
                action.started -= ManageCapturedAction;
                action.performed -= ManageCapturedAction;
            }
            action.Disable();
        }
    }


    /// <summary>
    /// Tracks when the user presses any controller button, it ensures the ManageCapturedAction method logic runs once, this 
    /// prevents calling the method multiple times when user continously or intermittently presses the controller button. 
    /// This bool is used externally in the LearnVrControllers1 class which it controls when the method logic triggers.
    /// </summary>
    [HideInInspector] public bool userPressed;
    void ManageCapturedAction(InputAction.CallbackContext context)
    {
        //if (context.action.type == InputActionType.Button && context.phase == InputActionPhase.Performed)
        //{
        //    if (userPressed) return;

        //    userPressed = true;
        //    pressedKey = context.action.name;
        //    Debug.Log($"Key pressed: {pressedKey}");
        //}

        //else if (context.action.type == InputActionType.Value && (context.phase == InputActionPhase.Started || context.phase == InputActionPhase.Started)) // && context.action.name == "Move")
        //{

        //    Debug.Log($"{context.action.name} was pressed.");
        //    //Vector2 values = context.ReadValue<Vector2>();
        //    //Debug.Log($"vector pressed: {values}");
        //}

        userPressed = true;
        pressedKey = context.action.name;
        Debug.Log($"Key pressed: {pressedKey}");

    }
}
