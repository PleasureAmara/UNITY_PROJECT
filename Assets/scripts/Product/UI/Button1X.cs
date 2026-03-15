using UnityEngine;
using localizer.utilities.buttons;

namespace localizer.product.ui
{
    public class Button1X : ButtonBase
    {
        [SerializeField] private UIManager _uiManager;
        public override void ClickAction()
        {
            _uiManager.LoadAnyPage($"{_button1}page");
        }
 
    }
}


