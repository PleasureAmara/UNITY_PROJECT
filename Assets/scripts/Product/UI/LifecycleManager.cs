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
    public class LifecycleManager : MonoBehaviour
    {
        /// <summary>
        /// the purpose of this instance is to control when the shouldSkipIntro bool variable should 
        /// trigger. 
        /// </summary>
        [SerializeField] private IntroductionManager introductionManager;

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
        /// Tracks the position of player at the time they activate the menu screen.
        /// </summary>
        private Vector3 playerPosition;

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
            stopButton.selectEntered.RemoveListener(ManageStop);
            restartButton.selectEntered.RemoveListener(ManageRestart);
            skipIntroButton.selectEntered.RemoveListener(ManageSkipIntro);
        }

        void OnMenuPress(InputAction.CallbackContext context)
        {
            if (isMenuActive) return;

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
            introductionManager.shouldSkipIntro = true;
            CloseMenu();
        }

        void CloseMenu(SelectEnterEventArgs args = null)
        {
            menuScreen.SetActive(false);
            isMenuActive = false;
        }
    }

}
