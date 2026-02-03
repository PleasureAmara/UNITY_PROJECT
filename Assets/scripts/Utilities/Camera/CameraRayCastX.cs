using localizer.core.interfaces;
using UnityEngine;
using UnityEngine.InputSystem;


namespace localizer.utilities.camera
{
    /// <summary>
    /// this class enables manual raycasting. it targets all gameobjects which inherit from the IClickable interface.
    /// </summary>
    public class CameraRayCastX : MonoBehaviour
    {
        //control the maximum distance a ray can go. prevents rays through walls. its passed as a parameter to the Raycast()
        public float m_RayCastDistance;

        void Update()
        {

            //if (Mouse.current.leftButton.wasPressedThisFrame)
            if (!Input.GetMouseButtonDown(0)) return;

            //Vector2 mousePosition = Mouse.current.position.ReadValue();
            var mousePosition = Input.mousePosition;
            //generate a ray
            if (mousePosition != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(mousePosition);

                //this will contain the instance returned by the Raycast() of PhyX engine. with this instance we can get more 
                //information on the events which happened during at the hit position.

                RaycastHit[] allHits = Physics.RaycastAll(ray);

                foreach (RaycastHit hit in allHits)
                {
                    Debug.Log($"{hit.collider.name} detected");
                    IClickable colliderScript = hit.collider.GetComponent<IClickable>();

                    if (colliderScript != null)
                    {
                        //colliderScript.ClickAction();
                        Debug.Log("Something fishy.");
                        //break;
                    }
                    //else
                    //    Debug.Log($"no script of type IClickable found on gameobject {hit.collider.name}");
                }
            }
            
            //}
            //else Debug.Log("cant find mouse position");

                //if (!isFound)
                //    Debug.Log($"no script of type IClickable found on gameobject out of {allHits.Length}");

          
        }
    }
}

