using System;
using System.Collections;
using UnityEngine;

namespace localizer.product.airplane
{

    public class AirplaneTaxi : MonoBehaviour
    {
        [SerializeField] private float taxiSpeed = 15.0f;
        [SerializeField] private float holdPositionLimit = -70.0f;

        [SerializeField] GameObject[] aircraftRotors;
        [SerializeField] private float rotorSpeed = 50.0f;
        private bool isRotorRotating;

        public void StartTaxing()
        {
            isRotorRotating = true;
            StartCoroutine(TaxiAircraft());
            StartCoroutine(ControlRotorMovement()); 
        }

        IEnumerator TaxiAircraft()
        {
            Debug.Log($"Starting position: {transform.position.z}");
            Debug.Log($"Hold limit: {holdPositionLimit}");
            Debug.Log($"Condition check: {transform.position.z < holdPositionLimit}");

            while (transform.position.z < holdPositionLimit)
            {
                Debug.Log($"Moving... Current z: {transform.position.z}");
                transform.Translate(taxiSpeed * Time.deltaTime * Vector3.forward, Space.Self);
                yield return null;
            }

            Debug.Log($"Stopped at z: {transform.position.z}");
        }

        IEnumerator ControlRotorMovement()
        {
            while (isRotorRotating)
            {
                foreach (var rotor in aircraftRotors)
                {
                    rotor.transform.Rotate(rotorSpeed * Time.deltaTime * Vector3.forward);
                }
                yield return null;
            }
        }

        public void DestroyAircraft()
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }

        private bool checkAirCraftPosition()
        {
            return transform.position.z < holdPositionLimit;
        }
    }
}
