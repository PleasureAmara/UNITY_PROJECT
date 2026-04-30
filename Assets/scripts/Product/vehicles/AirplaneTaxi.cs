using System;
using System.Collections;
using UnityEngine;

namespace localizer.product.airplane
{

    public class AirplaneTaxi : MonoBehaviour
    {
        [SerializeField] private float taxiSpeed =7.0f;
        [SerializeField] private float holdPositionLimit = 245.0f;

        [SerializeField] GameObject[] aircraftRotors;
        [SerializeField] private float rotorSpeed = 700.0f;

        [HideInInspector] public bool finishedTaxing; 
        
        void Start()
        {
            finishedTaxing = false;
            StartCoroutine(TaxiAircraft());
        }
        private void Update()
        {
            RotateRotors();
            
        }

        //the 0.001 in transform.Rotate(0,0.001f,0) is for correction due to the taxiway not being a perpendicular line, ]
        //moving straight takes the aircraft off the taxiway.
        IEnumerator TaxiAircraft()
        {
            while (transform.position.z < holdPositionLimit)
            {
                transform.Translate(taxiSpeed * Time.deltaTime * Vector3.forward, Space.Self);
                transform.Rotate(0, 0.001f, 0);
                yield return null;
            }
            while (transform.position.x < 1380)
            {
                transform.Translate(taxiSpeed * Time.deltaTime * Vector3.forward, Space.Self);
                transform.Rotate(0, 0.07f, 0);
                yield return null;
            }
            while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, 179.9f)) > 0.09)
            {
                Debug.Log($"y-angle: {transform.eulerAngles.y}");
                transform.Rotate(0, 0.09f, 0);
                yield return null;
            }

            transform.rotation = Quaternion.Euler(0, 180, 0);
            finishedTaxing = true;
        }


        void RotateRotors()
        {
            foreach (var rotor in aircraftRotors)
            {
                rotor.transform.Rotate(rotorSpeed * Time.deltaTime * Vector3.forward);
            }
        }

        public void DestroyAircraft()
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }

    }
}
