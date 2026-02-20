using UnityEngine;
using UnityEngine.InputSystem;

public class triggerAction : MonoBehaviour
{
    public InputActionReference trigAction;
    void OnEnable()
    {
        trigAction.action.Enable();
    }

    private void OnDisable()
    {
        trigAction.action.Disable();
    }

    private void Update()
    {
        if (trigAction.action.WasPressedThisFrame()) 
        {
            Debug.Log("Trigger pressed");
        }
    }


}
