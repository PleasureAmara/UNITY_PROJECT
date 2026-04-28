using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace localizer.product.airplane
{

    public class AirplaneTaxi : MonoBehaviour
    {
        AirplaneTaxi instance;
        [SerializeField] private float taxiSpeed = 7.0f;
        [SerializeField] private float holdPositionLimit = 250.0f;

        [SerializeField] GameObject[] aircraftRotors;
        [SerializeField] private bool hasRotors = false;
        [SerializeField] private float rotorSpeed = 50.0f;
        public bool finishedTaxing;

        private void Awake()
        {
            instance = this;
        }

        //public void StartTaxing()
        //{
        //    Debug.Log("Taxing started.");
        //    finishedTaxing = false;
        //    StartCoroutine(TaxiAircraft());
        //}

        private void Start()
        {
            Debug.Log("Taxing started.");
            finishedTaxing = false;
            StartCoroutine(TaxiAircraft());
        }

        private void Update()
        {
            if (hasRotors) RotateRotors();
        }

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
            while (Math.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, 179.9f)) > 0.09)
            {
                transform.Rotate(0, 0.09f, 0);
                yield return null;
            }

            transform.rotation = Quaternion.Euler(0,180,0);
            finishedTaxing = true;
        }

        private void RotateRotors()
        {
            foreach (var rotor in aircraftRotors)
            {
                rotor.transform.Rotate(rotorSpeed * Time.deltaTime * Vector3.forward);
            }
        }
    }
}
