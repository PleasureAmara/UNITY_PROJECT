using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace localizer.product.vehicle
{
    public class AirplaneTakeOff : MonoBehaviour
    {
        /// we use the general logic of NavigateVehicle class to move the aircrafts too. this is the role of this instance.
        [SerializeField] private NavigateVehicle navigateVehicle;

        //[SerializeField] private float maxTakeOffSpeed = 30.0f;
        //[SerializeField] private float acceleration = 1.0f;
        [SerializeField] private float climbSpeed = 8.0f;
        [SerializeField] private float startClimbLimit = 50.0f;

        private readonly float visualLimitZ = -2500.0f;
        [HideInInspector] public bool isAircraftVisual;
        
        public void StartTakeOff()
        {
            StartCoroutine(TakeOffTh17());
            StartCoroutine(TrackAircraft());
        }

        //IEnumerator TakeOffManager()
        //{
        //    while (isAircraftVisual)
        //    {
        //        transform.Translate(takeOffSpeed * Time.deltaTime * Vector3.forward, Space.Self);
        //        takeOffSpeed += acceleration;

        //        if (transform.position.z < startClimbLimit)
        //        {
        //            transform.Rotate(climbSpeed * Time.deltaTime * Vector3.left);
        //        }
        //        yield return null;
        //    }

        //}


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