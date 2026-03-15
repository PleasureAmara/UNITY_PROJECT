using UnityEngine;
using localizer.utilities.buttons;

namespace localizer.product.ui
{
    public class Button2X : ButtonBase
    {
        [SerializeField] private UIManager _uiManager;

        public override void ClickAction()
        {
            _uiManager.LoadAnyPage($"{_button2}page");
        }
    }
}


