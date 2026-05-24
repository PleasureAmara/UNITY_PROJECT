using localizer.product.vehicle;
using System;
using System.Collections;
using UnityEngine;

namespace localizer.product.airplane
{

    public class AirplaneTaxi : MonoBehaviour
    {
        [SerializeField] private float taxiSpeed =7.0f;
        [SerializeField] private float holdPositionLimitZ = 277.0f;

        [SerializeField] GameObject[] aircraftRotors;
        [SerializeField] private float rotorSpeed = 700.0f;

        /// we use the general logic of NavigateVehicle class to move the aircrafts too. this is the role of this instance.
        [SerializeField] private NavigateVehicle navigateVehicle;
        [SerializeField] private BasePivot taxiRwyPivot;

        [HideInInspector] public bool finishedTaxing; 
        
        public void StartTaxi()
        {
            finishedTaxing = false;
            StartCoroutine(TaxiAircraftAlongBravo());
        }
        private void Update()
        {
            RotateRotors();
            
        }

        IEnumerator TaxiAircraftAlongBravo()
        {
            while (transform.position.z < holdPositionLimitZ)
            {
                transform.Translate(taxiSpeed * Time.deltaTime * Vector3.forward, Space.Self);
                yield return null;
            }
            navigateVehicle.hasFinishedTurning = true;
            navigateVehicle.TurnVehicle(pivot:  taxiRwyPivot);

            while (!navigateVehicle.hasFinishedTurning)
            {
                yield return null;
            }

            //while (transform.position.x < 1380)
            //{
            //    transform.Translate(taxiSpeed * Time.deltaTime * Vector3.forward, Space.Self);
            //    transform.Rotate(0, 0.07f, 0);
            //    yield return null;
            //}
            //while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, 179.9f)) > 0.09)
            //{
            //    Debug.Log($"y-angle: {transform.eulerAngles.y}");
            //    transform.Rotate(0, 0.09f, 0);
            //    yield return null;
            //}

            transform.rotation = Quaternion.Euler(0, 180, 0);
            finishedTaxing = true;
        }

        //IEnumerator TaxiAircraftAlongBravo()
        //{
        //    //reset the states 
        //    finishedTaxing = false;
        //    navigateVehicle.hasVehicleReached = false;

        //    Vector3 taxiHoldPosition = new Vector3(transform.position.x, transform.position.y, holdPositionLimitZ);
        //    StartCoroutine(navigateVehicle.MoveVehicleForward(
        //        stopPosition: taxiHoldPosition, 
        //        rotationPivot: taxiRwyPivot));
            
        //    while (!navigateVehicle.hasVehicleReached)
        //    {
        //        yield return null;
        //    }
        //    finishedTaxing = true;
        //}


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
