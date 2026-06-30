using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace localizer.product.vehicle
{
    public class AirplaneTakeOff : MonoBehaviour
    {
        /// we use the general logic of NavigateVehicle class to move the aircrafts too. this is the role of this instance.
        [SerializeField] private NavigateVehicle navigateVehicle;

        private float climbSpeed = 0.1f;
        private float startClimbLimit = -870.0f;

        private readonly float visualLimitZ = -6000.0f;
        [HideInInspector] public bool isAircraftVisual;
        
        public void StartTakeOff()
        {
            //StartCoroutine(TakeOffTh17());
            StartCoroutine(TakeOffManager());
            StartCoroutine(TrackAircraft());
        }

        IEnumerator TakeOffManager()
        {
            isAircraftVisual = true;
            StartCoroutine(navigateVehicle.AccelerateVehicle(navigateVehicle.maxForwardSpeed, navigateVehicle.acceleration));
            while (isAircraftVisual)
            {
                //Debug.Log($"Vehicle posistion z: {transform.position.z}");
                transform.Translate(navigateVehicle.vehicleSpeed * Time.deltaTime * Vector3.forward, Space.Self);

                if (transform.position.z < startClimbLimit)
                {
                    transform.Rotate(climbSpeed * Time.deltaTime * Vector3.left);
                }
                yield return null;
            }

        }


        IEnumerator TakeOffTh17()
        {
            isAircraftVisual = true;
            Vector3 visualLimit = new Vector3(transform.position.x, transform.position.y, visualLimitZ);

            StartCoroutine(navigateVehicle.MoveVehicleForward(stopPosition: visualLimit));
            while (isAircraftVisual)
            {
                if (transform.position.z < startClimbLimit)
                {
                    transform.Rotate(climbSpeed * Time.deltaTime * Vector3.left);
                }
                yield return null;
            }
        }

        IEnumerator TrackAircraft()
        {
            while (transform.position.z > visualLimitZ)
            {
                yield return null;
            }
            isAircraftVisual = false;
        }

        public void DestroyAircraft()
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }
}