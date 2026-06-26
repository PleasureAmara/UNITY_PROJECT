using localizer.utilities.buttons;
using TMPro;
using UnityEngine;

namespace localizer.product.ui
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] _screens;
        private GameObject _activeScreen;
        private bool isScreenFound;

        private void Start()
        {
            LoadStatusPage();
        }

        internal void LoadAnyPage(string pageName)
        {
            isScreenFound = false;

            foreach (var screen in _screens)
            {
                if (screen.gameObject.tag == pageName)
                {
                    screen.gameObject.SetActive(true);

                    //store the active screen for usage in giving value to the buttons.
                    _activeScreen = screen;
                    isScreenFound = true;
                }
                else
                {
                    screen.gameObject.SetActive(false);
                }
            }
            if (!isScreenFound) 
                Debug.Log("Didnt find the screen you're  looking for.");
            else
                AssignButtonNames();

        }

        private void AssignButtonNames()
        {
            if (_activeScreen != null)
            {
                //get all the texts showing the upcoming screen names
                Transform button1 = _activeScreen.transform.Find("buttonText1");
                Transform button2 = _activeScreen.transform.Find("buttonText2");
                Transform button3 = _activeScreen.transform.Find("buttonText3");
                Transform button4 = _activeScreen.transform.Find("buttonText4");

                //set the texts to the buttons.
                //TODO: we dont have to check if these buttons are null, cause they will not be, but at this moment
                //further pressing of the button leads to pages not created yet, thus when we reach those pages, we shall log the
                //status screen for now. 

                ButtonBase._button1 = button1 != null? button1.gameObject.GetComponent<TextMeshProUGUI>().text.ToLower().Trim(): "status";
                ButtonBase._button2 = button2 != null? button2.gameObject.GetComponent<TextMeshProUGUI>().text.ToLower().Trim(): "status"; 
                ButtonBase._button3 = button3 != null? button3.gameObject.GetComponent<TextMeshProUGUI>().text.ToLower().Trim(): "status";
                ButtonBase._button4 = button4 != null? button4.gameObject.GetComponent<TextMeshProUGUI>().text.ToLower().Trim(): "status";
            }

        }

        private void LoadStatusPage()
        {
            LoadAnyPage("statuspage");
        }

    }
}


