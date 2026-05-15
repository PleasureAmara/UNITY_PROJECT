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

    [HideInInspector] public bool userPressed;
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
            //if (action.type == InputActionType.Button)
            //{
                action.performed += ManageCapturedAction;
            //}
        }
    }

    void UnMapActionMapToEventMethod(InputActionMap actionMap)
    {
        foreach (var action in actionMap.actions)
        {
            action.performed -= ManageCapturedAction;
        }
    }
    void ManageCapturedAction(InputAction.CallbackContext context)
    {
        if (userPressed) return; 

        userPressed = true;
        pressedKey = context.action.name;
        Debug.Log($"Key pressed: {context.action.name}");


    }
}
