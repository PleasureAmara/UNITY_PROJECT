using TMPro;
using UnityEngine;

namespace localizer.product.descriptions
{
    public class ShowDescription : MonoBehaviour
    {
        [Tooltip("This is to be filled by the developer. It is a string, it must match key inside the ActionsDescriptions._allActionsDictionary. Its the title which holds the words to be displayed to the user in the game")]
        [SerializeField] 
        private string actionToPerform;

        [Tooltip("The screen gameobject inside the canvas used for rendering text descriptions")]
        [SerializeField] 
        public GameObject descriptionScreen;

        [SerializeField] private TextMeshProUGUI textInScreen;

        //public void RenderScreen()
        //{
        //    //the variable pressedKey stores the key pressed by the user on the controller. 

        //    var targetDescription = ActionsDescriptions.FindDescription(actionToPerform, out string pressedKey);
        //    if (string.IsNullOrEmpty(targetDescription))
        //    {
        //        Debug.LogError($"The search key: '{targetDescription}' doesnt exist.");
        //        return;
        //    }
        //    //var targetAction = ActionsDescriptions._allActionsArray.First(a => a.Name == actionToPerform);

        //    textInScreen.text  = targetDescription;
        //    descriptionScreen.SetActive(true);
           
        //}

    }
}

