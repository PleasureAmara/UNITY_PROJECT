using localizer.core.interfaces;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
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
        private GameObject canvasScreen;

        //[Tooltip("Drag the XR ORIGIN gameobject here.")]
        //[SerializeField] private Transform xrOriginTransform;

        public void RenderScreen()
        {
            var targetAction = ActionsDescriptions._allActionsArray.First(a => a.Name == actionToPerform);

            canvasScreen.GetComponentInChildren<TextMeshProUGUI>().text  = targetAction.Description;

            //canvasScreen.GetComponentInParent<Transform>().position = Camera.main.transform.forward *;
            //canvasScreen.transform.position = Camera.main.transform.position;  + new Vector3(2,0,0);
            //canvasScreen.transform.position = (canvasScreen.transform.position - transform.position).normalized;
            //canvasScreen.transform.rotation = transform.localRotation;
            //canvasScreen.transform.LookAt(Camera.main.transform);

            //canvasScreen.transform.position = xrOriginTransform.position + new Vector3(1, 0, 0);
            canvasScreen.SetActive(true);
           
        }

    }
}

