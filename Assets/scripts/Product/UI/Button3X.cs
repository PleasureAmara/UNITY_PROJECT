using UnityEngine;
using localizer.utilities.buttons;

namespace localizer.product.ui
{
    public class Button3X : ButtonBase
    {
        [SerializeField] private UIManager _uiManager;

        public override void ClickAction()
        {
            _uiManager.LoadAnyPage($"{_button3}page");
        }

    }
}


