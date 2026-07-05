using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using System;

using localizer.product.player;


namespace localizer.product.ui
{
    /// <summary>
    /// Tracks only a single button on the Left controller i.e menu, once this button is clicked, everything that appears 
    /// afterwards are controlled by this class. 
    /// </summary>
    public class LifecycleManager : MonoBehaviour
    {
        /// <summary>
        /// the purpose of this event is to control the introduction logic IntroductionManager script
        /// </summary>
        //[SerializeField] private IntroductionManager introductionManager;
        public event Action shouldSkipIntro;

        [Tooltip("Add the character controller component attached to the XR origin")]
        [SerializeField] private CharacterController characterController;

        [Header("Input Actions")]
        [SerializeField] private InputAction menuButton;

        [Header("TeleportationAnchor variables")]
        [SerializeField] private TeleportPlayer teleportPlayer;
        [SerializeField] private TeleportationAnchor menuAnchor;

        [Header("Canvas variables")]
        [SerializeField] private GameObject menuScreen;
        [SerializeField] private XRSimpleInteractable stopButton;
        [SerializeField] private XRSimpleInteractable restartButton;
        [SerializeField] private XRSimpleInteractable skipIntroButton;
        [SerializeField] private XRSimpleInteractable cancelMenuButton;

        /// <summary>
        /// Tracks the state of the menu screen
        /// </summary>
        private bool isMenuActive;

        /// <summary>
        /// Tracks the position of player at the time they activate the menu screen. So that once they press exit, they are returned 
        /// back to their original position and orientation. 
        /// </summary>
        private Vector3 playerPosition;
        private Quaternion playerRotation;

        private void OnEnable()
        {
            menuButton.Enable();
            menuButton.performed += OnMenuPress;

            stopButton.selectEntered.AddListener(ManageStop);
            restartButton.selectEntered.AddListener(ManageRestart);
            skipIntroButton.selectEntered.AddListener(ManageSkipIntro);
            cancelMenuButton.selectEntered.AddListener(CloseMenu);
        }

        private void OnDisable()
        {
            menuButton.Disable();
            menuButton.performed -= OnMenuPress;

            stopButton.selectEntered.RemoveListener(ManageStop);
            restartButton.selectEntered.RemoveListener(ManageRestart);
            skipIntroButton.selectEntered.RemoveListener(ManageSkipIntro);
        }

        private void Start()
        {
            if (characterController == null)
            {
                Debug.LogError("There is no attached character controller.");
                return;
            }
        }

        void OnMenuPress(InputAction.CallbackContext context)
        {
            if (isMenuActive) return;

            //save the position of player before they are teleported.
            playerPosition = characterController.transform.position;
            playerRotation = characterController.transform.rotation;

            //teleport to the menu anchor and after that start the actions in the lambda expression.
            StartCoroutine(TeleportToMenuPosition(
                () =>
                {
                    //pause the game
                    Time.timeScale = 0f;
                    menuScreen.SetActive(true);
                    isMenuActive = true;
                }));
        }

        IEnumerator TeleportToMenuPosition(Action actionAfterTeleport)
        {
            teleportPlayer.hasTeleported = false;
            teleportPlayer.RequestToTeleportToAnchor(menuAnchor);
            while (!teleportPlayer.hasTeleported)
            {
                yield return null; 
            }
            actionAfterTeleport();  
        }

        void ManageStop(SelectEnterEventArgs args)
        {
            Application.Quit();
        }

        void ManageRestart(SelectEnterEventArgs args)
        {
            // its assumed that localizer110 will always be scene 0. 
            SceneManager.LoadScene(0);
            CloseMenu();
        }

        void ManageSkipIntro(SelectEnterEventArgs args)
        {
            //introductionManager.shouldSkipIntro = true;
            shouldSkipIntro?.Invoke();
            CloseMenu();
        }

        void CloseMenu(SelectEnterEventArgs args = null)
        {
            menuScreen.SetActive(false);
            isMenuActive = false;

            //place player
            characterController.transform.SetPositionAndRotation(playerPosition, playerRotation);
        }
    }

}
