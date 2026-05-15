using System.Collections;
using UnityEngine;
using localizer.core.interfaces;

namespace localizer.product.vehicle
{
    public class BasePivot : MonoBehaviour, IPivot
    {
        [SerializeField] private float maxRotationY;
        [SerializeField] private int finalRotationY;
        [HideInInspector] public NavigateVehicle attachedVehicleScript;

        private readonly float rotationSpeed = 4.0f;

        public IEnumerator RotatePivot()
        {
            Quaternion maxRotation = Quaternion.Euler(0, maxRotationY, 0);

            if (attachedVehicleScript != null)
            {
                attachedVehicleScript.hasFinishedTurning = false;
                attachedVehicleScript.gameObject.transform.SetParent(transform, worldPositionStays: true);

                while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, maxRotationY)) > 0.05)
                {
                    //transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, maxRotation, rotationSpeed*Time.deltaTime);
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
