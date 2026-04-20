using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace localizer.product.airplane
{
    public class AirplaneTakeOff : MonoBehaviour
    {
        [SerializeField] private float takeOffSpeed = 10.0f;
        [SerializeField] private float acceleration = 1.0f;
        [SerializeField] private float climbSpeed = 5.0f;
        [SerializeField] private float startClimbLimit = 50.0f;
        private readonly float visualLimit = -900.0f;
        private bool isAircraftVisual;
        
        public void StartTakeOff()
        {
            isAircraftVisual = true;
            StartCoroutine(TakeOffManager());
        }

        IEnumerator TakeOffManager()
        {
            while (isAircraftVisual)
            {
                transform.Translate(takeOffSpeed * Time.deltaTime * Vector3.forward, Space.Self);
                takeOffSpeed += acceleration;

                if (transform.position.z < startClimbLimit)
                {
                    Debug.Log("climbing speed reached");
                    transform.Rotate(climbSpeed * Time.deltaTime * Vector3.left);
                }
                yield return null;
            }
        }

        public void DestroyAircraft()
        {
            if (transform.position.z < visualLimit)
            {
                StopAllCoroutines();
                Destroy(gameObject);
            }
        }
    }
}