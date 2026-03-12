using UnityEngine;
using localizer.utilities.buttons;

namespace localizer.product.ui
{
    public class Button1X : ButtonBase
    {
        [SerializeField] private UIManager _uiManager;
        private void Start()
        {
            //_buttonAction1.performed += context => ClickAction();
        }
        public override void ClickAction()
        {
            Debug.Log($"Button1 clicked opening {_button1}page");
            _uiManager.LoadAnyPage($"{_button1}page");
        }
 
    }
}


