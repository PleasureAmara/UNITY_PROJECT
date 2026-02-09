using UnityEngine;
using TMPro;
using localizer.utilities.buttons;

// test 
using localizer.product.led;

namespace localizer.product.ui
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] _screens;

        private GameObject _activeScreen;
        
        [SerializeField] private OnOffLEDManager lEDManager;

        private void Start()
        {
            LoadStatusPage();

            //sample code to blink the light
            //lEDManager.isEmissionEnabled = true;
            //lEDManager.isBlinking = false;
        }

        internal void LoadAnyPage(string pageName)
        {
            foreach (var screen in _screens)
            {
                if (screen.gameObject.tag == pageName)
                {
                    screen.gameObject.SetActive(true);

                    //store the active screen for usage in giving value to the buttons.
                    _activeScreen = screen;
                }
                else
                {
                    screen.gameObject.SetActive(false);
                }
                
            }

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

                //set the texts to the buttons
                if (button1 != null) ButtonBase._button1 = button1.gameObject.GetComponent<TextMeshProUGUI>().text.ToLower().Trim();
                if (button2 != null) ButtonBase._button2 = button2.gameObject.GetComponent<TextMeshProUGUI>().text.ToLower().Trim();
                if (button3 != null) ButtonBase._button3 = button3.gameObject.GetComponent<TextMeshProUGUI>().text.ToLower().Trim();
                if (button4 != null) ButtonBase._button4 = button4.gameObject.GetComponent<TextMeshProUGUI>().text.ToLower().Trim();

            }

        }

        private void LoadStatusPage()
        {
            LoadAnyPage("statuspage");
        }

    }
}


