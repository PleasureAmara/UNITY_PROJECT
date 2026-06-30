using System.Collections;
using UnityEngine;
using localizer.core.interfaces;

namespace localizer.product.vehicle
{
    public class SteeringMovement
    {
        
        //[SerializeField] public GameObject steeringDirection;
    }
    public class BasePivot : MonoBehaviour
    {
        [SerializeField] private float maxRotationY;
        [SerializeField] private int finalRotationY;
        //[SerializeField] private GameObject steeringPivot;
        [HideInInspector] public NavigateVehicle attachedVehicleScript;

        private readonly float rotationSpeed = 4.0f;

        //used to record the initial steering position
        Quaternion maxSteeringRotationLocal;
        Quaternion minSteeringRotationLocal;

        public IEnumerator RotatePivot(GameObject steeringPivot = null)
        {
            Quaternion maxRotation = Quaternion.Euler(0, maxRotationY, 0);
            
            //Quaternion maxSteeringRotation = Quaternion.Euler(steeringPivot.transform.eulerAngles.x, steeringPivot.transform.eulerAngles.y, maxRotationY/2);
            //Quaternion minSteeringRotation = Quaternion.Euler(steeringPivot.transform.eulerAngles.x, steeringPivot.transform.eulerAngles.y, 0);

            if (steeringPivot != null)
            {
                maxSteeringRotationLocal = Quaternion.Euler(steeringPivot.transform.localEulerAngles.x, steeringPivot.transform.localEulerAngles.y, maxRotationY / 2);
                minSteeringRotationLocal = Quaternion.Euler(steeringPivot.transform.localEulerAngles.x, steeringPivot.transform.localEulerAngles.y, 0);

            }

            //Debug.Log($"Max steering rotation in z: {maxSteeringRotationLocal.eulerAngles.z}");
            //Debug.Log($"max - initial steering Rotation difference in z: ({Mathf.Abs(Mathf.DeltaAngle(steeringPivot.transform.localEulerAngles.z, maxSteeringRotationLocal.eulerAngles.z))})");
            //Debug.Log($"max - initial car Rotation difference in z: ({Mathf.Abs(Mathf.DeltaAngle(transform.localEulerAngles.y, maxRotationY))})");

            if (attachedVehicleScript != null)
            {
                attachedVehicleScript.hasFinishedTurning = false;
                attachedVehicleScript.gameObject.transform.SetParent(transform, worldPositionStays: true);

                while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, maxRotationY)) > 0.05)
                {
                    //transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, maxRotation, rotationSpeed*Time.deltaTime);
                    
                    //add steering movement
                    if (steeringPivot  != null)
                    {
                        if (Mathf.Abs(Mathf.DeltaAngle(steeringPivot.transform.localEulerAngles.z, maxSteeringRotationLocal.eulerAngles.z)) > 0.05)
                        {
                            //steeringPivot.transform.rotation = Quaternion.RotateTowards(steeringPivot.transform.rotation, maxSteeringRotation, rotationSpeed * Time.deltaTime);
                            steeringPivot.transform.localRotation = Quaternion.RotateTowards(steeringPivot.transform.localRotation, maxSteeringRotationLocal, rotationSpeed * Time.deltaTime);
                        }
                        else
                        {
                            //if the steering reaches half the turn, then start moving the steering back to the original rotation.
                            //maxSteeringRotation = minSteeringRotation;
                            maxSteeringRotationLocal = minSteeringRotationLocal;
                        }
                            

                        //Debug.Log($"steering instanteneous rotation: {steeringPivot.transform.eulerAngles}");
                    }
                    
                    yield return null;
                }
                transform.rotation = Quaternion.Euler(0, finalRotationY, 0);

                attachedVehicleScript.gameObject.transform.SetParent(null);
                attachedVehicleScript.hasFinishedTurning = true;
            }
            else
            {
                Debug.LogError("There is no attached gameObject to rotate, exiting coroutine.");
            }


        }
    }
}
