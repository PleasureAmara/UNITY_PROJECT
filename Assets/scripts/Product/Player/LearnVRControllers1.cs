using TMPro;
using UnityEngine;
using UnityEngine.UI;

using localizer.product.descriptions;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Threading;

namespace localizer.product.player
{
    public class LearnVRControllers1 : MonoBehaviour
    {
        [SerializeField] private TrackerUserInput trackerUserInput;

        [Header("Canvas items")]
        [SerializeField] private Canvas itemsCanvas;

        //[Tooltip("Under the introduction panel in the items canvas, drag the title gameobject.")]
        [SerializeField] private TextMeshProUGUI title;

        //[Tooltip("Under the introduction panel in the items canvas, drag the content gameobject.")]
        [SerializeField] private TextMeshProUGUI content;
        [SerializeField] private TextMeshProUGUI footer;

        //[Tooltip("Under the introduction panel in the items canvas, drag the okay button gameobject.")]
        [SerializeField] private XRSimpleInteractable continueButton;
        [SerializeField] private XRSimpleInteractable tryAgainButton;

        /// <summary>
        /// Tobe tracked by the IntroductionManager script. 
        /// </summary>
        [HideInInspector] public bool isIntroFinished;

        private int count = 0;
        //private bool  isContinueButtonClicked;

        public void SetUpInitialState()
        {
            isIntroFinished = false;
            footer.gameObject.SetActive(false);
            //continueButton.gameObject.SetActive(false);

            if (continueButton != null) //&& tryAgainButton != null)
            {
                // this setting prevents two listeners on  asingle button. i.e. before we set a listener, we make sure all existing listeners are removed.
                continueButton.selectEntered.RemoveAllListeners();
                continueButton.selectEntered.AddListener(Continue);
                tryAgainButton.selectEntered.RemoveAllListeners();
                tryAgainButton.selectEntered.AddListener(TryAgain);


                //run the stage for the first time. 
                ManageContent();
                return;
            }
            Debug.LogError("The canvas attached has no CONTINUE or TRY AGAIN BUTTONS as a child game object");
        }

        //we removed the count from the arguments of ManageContent(int count) because the event onClick expects methods with no parameter.
        public void ManageContent()
        {
            switch (count)
            {
                case 0:
                    GetNextContent("Welcome to VR");
                    // show the introduction for one second and continue to next content. 
                    //StartCoroutine(() => {
                    //    int timeCount = 0;
                    //    while (timeCount < 5)
                    //    {
                    //        yield return new WaitForSeconds(1);
                    //        timeCount++;
                    //    }
                    //});
                    //Thread.Sleep(5000);
                    //count++;
                    //Continue();
                    break;
                case 1:
                    GetNextContent("Clicking buttons and switches");
                    //isContinueButtonClicked = false;
                    break;
                case 2:
                    GetNextContent("Rotating Around");
                    //isContinueButtonClicked= false;
                    //count++;
                    break;
                case 3:
                    GetNextContent("Opening and closing Doors");
                    //isContinueButtonClicked= false;
                    //count++;
                    break;
                case 4:
                    GetNextContent("Straight locomotion");
                    //isContinueButtonClicked = false;
                    //count++;
                    break;

                case 5:
                    GetNextContent("Final Conclusion");
                    isIntroFinished = true;
                    break;

            }
        }

        //TODO: make sure the failure button and footer only appears when you are in the game not when you're
        //pressing next.  
        private void GetNextContent(string searchKey)
        {
            //we set the variable userPressed to false because anytime any controller button is pressed, the field 
            //userPressed = true even in the case when we just click continue button. this implies while loop in 
            // the TrackUserEntry never runs because the condition is already met. To prevent this, we set the 
            //variable to false so we can listen for the user input. 
            //trackerUserInput.userPressed = false;

            //string pressedButton;
            string searchedContent = ActionsDescriptions.FindDescription(searchKey, out string relatedButton);
            if (string.IsNullOrEmpty(searchedContent))
            {
                Debug.LogError($"The search key: '{searchKey}' doesnt exist.");
                return;
            }
            if (content == null || title == null)
            {
                Debug.LogError("No title or content gameobjects attached.");
                return;
            }

            content.text = searchedContent;
            title.text = searchKey;

            if (relatedButton == null)
            {
                count++;
                return;
            }

            //disable the continue button
            continueButton.gameObject.SetActive(false);
            tryAgainButton.gameObject.SetActive(false);
            footer.gameObject.SetActive(false);

            //reset the track state
            trackerUserInput.userPressed = false;

            StartCoroutine(TrackUserEntry(
                actualEntry: relatedButton,
                ToDoIfUserPasses: () =>
                {
                    footer.color = Color.green;
                    footer.text = "That's right,";
                    footer.gameObject.SetActive(true);

                    //reset the track state
                    //trackerUserInput.userPressed = false;

                    continueButton.gameObject.SetActive(true);

                },
                ToDoIfUserFails: () =>
                {

                    //if (!isContinueButtonClicked)
                    //{

                    footer.color = Color.red;
                    footer.text = "Not the correct button,";
                    footer.gameObject.SetActive(true);
                    tryAgainButton.gameObject.SetActive(true);
                    //trackerUserInput.userPressed = false;
                    //}
                    //else isContinueButtonClicked = false; 
                }
                ));

        }

        void TryAgain(SelectEnterEventArgs args = null)
        {
            ManageContent();
        }

        void Continue(SelectEnterEventArgs args = null)
        {
            //we set the variable userPressed to false because anytime any controller button is pressed, the field 
            //userPressed = true even in the case when we just click continue button. this implies while loop in 
            // the TrackUserEntry never runs because the condition is already met. To prevent this, we set the 
            //variable to false so we can listen for the user input. 
            //trackerUserInput.userPressed=false;
            count++;
            ManageContent();
        }

        IEnumerator TrackUserEntry(string actualEntry, Action ToDoIfUserPasses, Action ToDoIfUserFails)
        {
            Debug.Log($"has user pressed anything? {trackerUserInput.userPressed}");
            Debug.Log($"The true button: {actualEntry}");
            while (!trackerUserInput.userPressed)
            {
               yield return null;   
                
            }
            //wait for one frame before we check the trackerInput.pressedKey field. this is to make sure the field
            //is set  before we check it. 
            yield return null;

            if (trackerUserInput.pressedKey == actualEntry) ToDoIfUserPasses();
            else ToDoIfUserFails();
        }
    }
}
