using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace localizer.product.vehicle
{
    public class AirplaneTakeOff : MonoBehaviour
    {
        [SerializeField] private float takeOffSpeed = 10.0f;
        [SerializeField] private float acceleration = 1.0f;
        [SerializeField] private float climbSpeed = 8.0f;
        [SerializeField] private float startClimbLimit = 50.0f;

        private readonly float visualLimit = -900.0f;
        [HideInInspector] public bool isAircraftVisual;
        
        public void StartTakeOff()
        {
            isAircraftVisual = true;
            StartCoroutine(TakeOffManager());
            StartCoroutine(TrackAircraft());
        }

        IEnumerator TakeOffManager()
        {
            while (isAircraftVisual)
            {
                transform.Translate(takeOffSpeed * Time.deltaTime * Vector3.forward, Space.Self);
                takeOffSpeed += acceleration;

                if (transform.position.z < startClimbLimit)
                {
                    transform.Rotate(climbSpeed * Time.deltaTime * Vector3.left);
                }
                yield return null;
            }

        }

        IEnumerator TrackAircraft()
        {
            while (transform.position.z > visualLimit)
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