using TMPro;
using UnityEngine;
using UnityEngine.UI;

using localizer.product.descriptions;
using System.Linq;

namespace localizer.product.player
{
    public class LearnVRControllers1 : MonoBehaviour
    {
        [Tooltip("Under the introduction panel in the items canvas, drag the title gameobject.")]
        [SerializeField] private TextMeshProUGUI title;

        [Tooltip("Under the introduction panel in the items canvas, drag the content gameobject.")]
        [SerializeField] private TextMeshProUGUI content;

        [Tooltip("Under the introduction panel in the items canvas, drag the okay button gameobject.")]
        [SerializeField] private Button okayButton;

        private int count = 0;
        public bool isIntroFinished = false;

        public void SetUpInitialState()
        {
            // this setting prevents two listeners on  asingle button. i.e. before we set a listener, we make sure all existing listeners are removed.
            okayButton.onClick.RemoveAllListeners();
            okayButton.onClick.AddListener(ManageContent);

            ManageContent();
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
            content.text = searchedContent;
            title.text = searchKey;
            
        }
        //we removed the count from the arguments of ManageContent(int count) because the event onClick expects methods with no parameter.
        public void ManageContent()
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
