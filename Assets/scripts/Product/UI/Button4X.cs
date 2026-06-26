using UnityEngine;
using localizer.utilities.buttons;

namespace localizer.product.ui
{
    public class Button4X : ButtonBase
    {
        [SerializeField] private UIManager _uiManager;

        public override void ClickAction()
        {
            _uiManager.LoadAnyPage($"{_button4}page");
        }
    }
}


