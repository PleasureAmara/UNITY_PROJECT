using localizer.core.interfaces;
using UnityEngine;

namespace localizer.utilities
{
    public class MouseClicking : MonoBehaviour, IClickable
    {
        public void ClickAction()
        {
            Debug.Log($"{gameObject.name} was clicked.");
        }

       
    }
}

