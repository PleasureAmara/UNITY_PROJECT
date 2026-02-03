using UnityEngine;
using localizer.utilities.buttons;

namespace localizer.product.ui
{
    public class Button3X : ButtonBase
    {
        [SerializeField] private UIManager _uiManager;

        private void Start()
        {
            _buttonAction3.performed += context => ClickAction();
        }
        public override void ClickAction()
        {
            Debug.Log("Button3 clicked.");
            _uiManager.LoadAnyPage($"{_button3}page");
        }

        //void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.C))
        //    {
        //        ClickAction();
        //    }
        //}
    }
}


