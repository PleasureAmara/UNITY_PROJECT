using UnityEngine;
using localizer.utilities.buttons;

namespace localizer.product.ui
{
    public class Button4X : ButtonBase
    {
        [SerializeField] private UIManager _uiManager;

        private void Start()
        {
            //_buttonAction4.performed += context => ClickAction();
        }

        public override void ClickAction()
        {
            Debug.Log($"Button 4 clicked opening {_button4}page");
            if (_uiManager != null)
            {
                _uiManager.LoadAnyPage($"{_button4}page");
            }
            
        }

        //void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.V))
        //    {
        //        ClickAction();
        //    }
        //}
    }
}


