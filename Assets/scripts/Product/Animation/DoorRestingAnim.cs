using UnityEngine;

namespace localizer.product.animation 
{
    /// <summary>
    /// this class is registered with the animation event to trigger when the DoorClose animation 
    /// ends. it will transition the door from the closed state to resting state. 
    /// </summary>
    public class DoorRestingAnim : MonoBehaviour
    {
        void ControlDoorRest()
        {
            Animator animator = GetComponent<Animator>();
            animator.SetBool("isResting", true);
        }
    }
}



