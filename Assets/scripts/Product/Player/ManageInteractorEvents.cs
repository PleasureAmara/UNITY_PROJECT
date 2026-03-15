using localizer.product.descriptions;
using localizer.product.led;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace localizer.product.player
{
    public class ManageInteractorEvents : MonoBehaviour
    {
        [Header("Target interactible")]
        [Tooltip("Add game object with your target Interactable component.")]
        [SerializeField] private XRBaseInteractable xrInteractable;

        [Tooltip("Attach the Show Description Component.")]
        [SerializeField] private ShowDescription showDescriptionScript;

        ////tests
        //public OnOffLEDManager onOffLEDManager;
        ////tests

        private bool isAlreadySelected;

        private void Awake()
        {
            xrInteractable = GetComponent<XRBaseInteractable>();
        }

        private void OnEnable()
        {
            xrInteractable.selectEntered.AddListener(ManageSelectActions);
        }

        private void OnDisable()
        {
            xrInteractable.selectEntered.RemoveListener(ManageSelectActions);
        }

        void ManageSelectActions(SelectEnterEventArgs args)
        {
            ManageDescription();
        }

        void ManageDescription()
        {
            if (isAlreadySelected) return;

            showDescriptionScript.RenderScreen();
            isAlreadySelected = true;
        }

    }
}

