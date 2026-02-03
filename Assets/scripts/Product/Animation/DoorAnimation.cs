using localizer.core.interfaces;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace localizer.product.animation
{
    public class DoorAnimation : MonoBehaviour
    {
        [SerializeField] private Animator doorAnimator;

        [SerializeField] private InputActionAsset doorActionAsset;
        private InputActionMap doorActionMap;
        private InputAction doorAction;

        private void Awake()
        {
            doorActionMap = doorActionAsset.FindActionMap("GamePlay");
            doorAction = doorActionMap.FindAction("doorAction");

            doorAction.performed += context => MoveDoor();
        }

        private void OnEnable()
        {
            doorAction.Enable();
        }

        private void OnDisable()
        {
            doorAction.Disable();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (doorAnimator == null)
            {
                doorAnimator.SetBool("isResting", true);
            }
        }

        private void MoveDoor()
        {
            bool currentState = doorAnimator.GetBool("isOpen");
            doorAnimator.SetBool("isOpen", !currentState);

            if (!currentState)
            {
                doorAnimator.SetBool("isResting", false);
            }

        }

    }
}


