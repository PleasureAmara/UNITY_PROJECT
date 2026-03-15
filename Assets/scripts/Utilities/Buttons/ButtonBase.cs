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

        /// <summary>
        /// the method executes only when there is a click on the gameobject.
        /// </summary>
        /// 
        // we cant make the method abstract since it being inherited by the individual buttons which must be accessed
        // in the unity editor.
        //an abstract class cant be passed to gameobject in the editor.
        public virtual void ClickAction()
        {
            Debug.Log("Button Clicked but not performing as intended... To ensure your button performs well, override the method ClickAction with custom logic.");
        }
    }
}


