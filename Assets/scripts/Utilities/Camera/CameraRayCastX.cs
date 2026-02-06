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

        [SerializeField] private Camera m_Camera;

        void Update()
        {

            //if (Mouse.current.leftButton.wasPressedThisFrame)
            if (Input.GetMouseButtonDown(0))
            {
                //Vector2 mousePosition = Mouse.current.position.ReadValue();

                Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                //this will contain the instance returned by the Raycast() of PhyX engine. with this instance we can get more 
                //information on the events which happened during at the hit position.
                if (Physics.Raycast(ray, out hit))
                {

                    //RaycastHit[] allHits = Physics.RaycastAll(ray);

                    //foreach (RaycastHit hit in allHits)
                    //{
                    //Debug.Log($"{hit.collider.name} detected");

                    if (hit.collider.TryGetComponent<IClickable>(out var colliderScript))
                    {
                        //Debug.Log("Something fishy.");
                        colliderScript.ClickAction();

                        //break;
                    }
                    //IClickable colliderScript = hit.collider.GetComponent<IClickable>();

                    
                    //else
                    //    Debug.Log($"no script of type IClickable found on gameobject {hit.collider.name}");
                    //}
                }
            }
            
            //}
            //else Debug.Log("cant find mouse position");

                //if (!isFound)
                //    Debug.Log($"no script of type IClickable found on gameobject out of {allHits.Length}");

          
        }
    }
}

