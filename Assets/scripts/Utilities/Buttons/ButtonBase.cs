using UnityEngine;
using localizer.core.interfaces;
using UnityEngine.InputSystem;

namespace localizer.utilities.buttons
{
    /// <summary>
    /// the ButtonBase class is set for the UI buttons. this is because the buttons has repeating code. so having a single 
    /// base class is reasonable and makes the code readable.
    /// </summary>

    public class ButtonBase : MonoBehaviour, IClickable
    {
        public static string _button1;
        public static string _button2;
        public static string _button3;
        public static string _button4 = "status";
        //public string _buttonKeyCode;

        //[SerializeField] private InputActionAsset _actionAsset;
        //protected InputActionMap _actionMap;
        //protected InputAction _buttonAction1;
        //protected InputAction _buttonAction2;
        //protected InputAction _buttonAction3;
        //protected InputAction _buttonAction4;

        //void Awake()
        //{
        //    _actionMap = _actionAsset.FindActionMap("GamePlay");
        //    _buttonAction1 = _actionMap.FindAction("ScreenButton1");
        //    _buttonAction2 = _actionMap.FindAction("ScreenButton2");
        //    _buttonAction3 = _actionMap.FindAction("ScreenButton3");
        //    _buttonAction4 = _actionMap.FindAction("ScreenButton4");

        //}

        //void OnEnable()
        //{
        //    _buttonAction1.Enable();
        //    _buttonAction2.Enable();
        //    _buttonAction3.Enable();
        //    _buttonAction4.Enable();
        //}

        //void OnDisable()
        //{
        //    _buttonAction1.Disable();
        //    _buttonAction2.Disable();
        //    _buttonAction3.Disable();
        //    _buttonAction4.Disable();
        //}

        /// <summary>
        /// the method executes only when there is a click on the gameobject.
        /// </summary>
        /// 
        // we cant make the method abstract since we have to pass the InputActionAsset once for all child classes.
        //So, the button class has to be accessible in the unity editor. an abstract class cant be passed to 
        // gameobject in the editor.
        public virtual void ClickAction()
        {
            Debug.Log("Button Clicked but not performing as intended... To ensure your button performs well, override the method ClickAction with custom logic.");
        }
    }
}


