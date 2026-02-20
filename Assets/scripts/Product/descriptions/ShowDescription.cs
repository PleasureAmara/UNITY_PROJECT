using localizer.core.interfaces;
using TMPro;
using UnityEngine;

namespace localizer.product.descriptions
{
    public class ShowDescription : MonoBehaviour, IClickable
    {
        [SerializeField] private string equipmentName;
        [SerializeField] private GameObject canvasScreen;
        private bool isShowing= false;

        public void ClickAction()
        {
            if (equipmentName == null) return;

            if (!isShowing)
            {
                string textOnCanvas = canvasScreen.GetComponentInChildren<TextMeshProUGUI>().text;

                Debug.Log($"String passed {equipmentName}. ");
                textOnCanvas = AllEquipment.equipmentToDescribe[equipmentName];

                canvasScreen.transform.Translate(new Vector3(transform.position.x, transform.position.y + 5, transform.position.z));
                canvasScreen.SetActive(true);
                isShowing = true;
                return;
            }
            isShowing = false;
            canvasScreen.SetActive(false);


        }


    }
}

