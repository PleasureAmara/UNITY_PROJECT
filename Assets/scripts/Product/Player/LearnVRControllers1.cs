using TMPro;
using UnityEngine;
using UnityEngine.UI;

using localizer.product.descriptions;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

namespace localizer.product.player
{
    public class LearnVRControllers1 : MonoBehaviour
    {
        [SerializeField] private Canvas itemsCanvas;

        //[Tooltip("Under the introduction panel in the items canvas, drag the title gameobject.")]
        [SerializeField] private TextMeshProUGUI title;

        //[Tooltip("Under the introduction panel in the items canvas, drag the content gameobject.")]
        [SerializeField] private TextMeshProUGUI content;

        //[Tooltip("Under the introduction panel in the items canvas, drag the okay button gameobject.")]
        [SerializeField] private XRSimpleInteractable okayButton;

        private int count = 0;
        [HideInInspector]public bool isIntroFinished;

        public void SetUpInitialState()
        {
            isIntroFinished = false;
            if (okayButton != null)
            {
                // this setting prevents two listeners on  asingle button. i.e. before we set a listener, we make sure all existing listeners are removed.
                okayButton.selectEntered.RemoveAllListeners();
                okayButton.selectEntered.AddListener(ManageContent);

                ManageContent();
                return;
            }
            Debug.LogError("The canvas attached has no button as a child game object");
        }

        private void GetNextContent(string searchKey)
        {
            string searchedContent = ActionsDescriptions.FindDescription(searchKey);
            //string searchedContent = ActionsDescriptions._allActionsArray.First(item => item.Name == searchKey).Description;
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
        }
        //we removed the count from the arguments of ManageContent(int count) because the event onClick expects methods with no parameter.
        public void ManageContent(SelectEnterEventArgs args = null )
        {
            switch (count)
            {
                case 0:
                    GetNextContent("Welcome to VR");
                    count++;
                    break;
                case 1:
                    GetNextContent("Straight locomotion");
                    count++;
                    break;
                case 2:
                    GetNextContent("Rotating Around");
                    count++;
                    break;
                case 3:
                    GetNextContent("Opening and closing Doors");
                    count++;
                    break;
                case 4:
                    GetNextContent("Clicking buttons and switches");
                    count++;
                    break;

                case 5:
                    GetNextContent("Final Conclusion");
                    isIntroFinished = true;
                    break;

            }
        }
    }
}
