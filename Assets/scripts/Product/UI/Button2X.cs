using UnityEngine;
using localizer.utilities.buttons;

namespace localizer.product.ui
{
    public class Button2X : ButtonBase
    {
        [SerializeField] private UIManager _uiManager;

        private void Start()
        {
            _buttonAction2.performed += context => ClickAction();
        }
        public override void ClickAction()
        {
            Debug.Log($"Button2 clicked opening {_button2}page");
            _uiManager.LoadAnyPage($"{_button2}page");
        }

        //void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.X))
        //    {
        //        ClickAction();
        //    }
        //}
    }
}


