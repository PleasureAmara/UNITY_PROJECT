using localizer.core.enums;
using localizer.product.vehicle;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace localizer.product.airplane
{
    [Serializable]
    public class RotationPivots
    {
        public BasePivot apron1ToBravo;
        public BasePivot taxiRwyPivot;
    }

    public class AirplaneTaxi : MonoBehaviour
    {
        [SerializeField] private RotationPivots rotationPivots;

        /// we use the general logic of NavigateVehicle class to move the aircrafts too. this is the role of this instance.
        [Tooltip("Attach the script NavigateVehicle which is found on this aircraft.")]
        [SerializeField] private NavigateVehicle navigateVehicle;

        [Header("Aircraft movement parameters")]
        [SerializeField] GameObject[] aircraftRotors;
        private readonly float taxiSpeed = 15.0f; // original 7.0f
        private readonly float rotorSpeed = 700.0f;

        /// <summary>
        /// We use this boolean to controlk when the aircraft take off script starts running.
        /// </summary>
        [HideInInspector] public bool hasFinishedTaxing;

        private bool hasReachedHoldingPosition;
        Vector3 vehicleStartTaxiPosition;

        //private void Start()
        //{
        //    Debug.Log($"airplane start position: {transform.position}");
        //    vehicleStartTaxiPosition = new Vector3(1240, transform.position.y, transform.position.z);
        //    hasReachedHoldingPosition = false;
        //}

        public void StartTaxi()
        {
            vehicleStartTaxiPosition = new Vector3(1240, transform.position.y, transform.position.z);
            hasReachedHoldingPosition = false;

            hasFinishedTaxing = false;

            //move aircraft to the position where it will start turning towards taxiway Bravo.
            StartCoroutine(navigateVehicle.MoveVehicleForward(stopPosition:
                vehicleStartTaxiPosition));
            StartCoroutine(WaitForAnyCondition(
                () => navigateVehicle.hasVehicleReached,
                () =>
                {
                    //Debug.Log("Reached the start rotation position");
                    //reset the pivots rotation
                    rotationPivots.apron1ToBravo.transform.rotation = Quaternion.Euler(0, 0, 0);
                    rotationPivots.taxiRwyPivot.transform.rotation = Quaternion.Euler(0, 0, 0);
                    //position the pivot at exactly 45m from the center of the aircraft
                    rotationPivots.apron1ToBravo.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 45);
                    navigateVehicle.TurnVehicle(rotationPivots.apron1ToBravo);

                    //wait until the aircraft is turned perfectly, then taxi
                    StartCoroutine(WaitForAnyCondition(
                        () => navigateVehicle.hasFinishedTurning,
                        () =>
                        {
                            Vector3 taxiHoldingPosition = new Vector3(navigateVehicle.transform.position.x, navigateVehicle.transform.position.y, 278);
                            StartCoroutine(TaxiAircraft(taxiHoldingPosition));                            
                            StartCoroutine(WaitForAnyCondition(
                                () => hasReachedHoldingPosition,
                                () =>
                                {
                                    navigateVehicle.TurnVehicle(rotationPivots.taxiRwyPivot);
                                    StartCoroutine(WaitForAnyCondition(
                                        () => navigateVehicle.hasFinishedTurning,
                                        () => hasFinishedTaxing = true
                                    ));
                                }
                            ));
                        }
                    ));
                }
                ));

        }
        //private void Update()
        //{
        //    RotateRotors();

        //}

        IEnumerator TaxiAircraft(Vector3 holdingPosition)
        {

            while (Mathf.Abs(Vector3.Distance(transform.position, holdingPosition)) > 0.1)
            {
                transform.position = Vector3.MoveTowards(transform.position, holdingPosition, taxiSpeed * Time.deltaTime);
                yield return null;
            }
            hasReachedHoldingPosition = true;
        }
        //    while (transform.position.z < holdPositionLimitZ)
        //    {
        //        transform.Translate(taxiSpeed * Time.deltaTime * Vector3.forward, Space.Self);
        //        yield return null;
        //    }
        //    navigateVehicle.hasFinishedTurning = true;
        //    navigateVehicle.TurnVehicle(pivot:  taxiRwyPivot);

        //    while (!navigateVehicle.hasFinishedTurning)
        //    {
        //        yield return null;
        //    }

        //    //while (transform.position.x < 1380)
        //    //{
        //    //    transform.Translate(taxiSpeed * Time.deltaTime * Vector3.forward, Space.Self);
        //    //    transform.Rotate(0, 0.07f, 0);
        //    //    yield return null;
        //    //}
        //    //while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, 179.9f)) > 0.09)
        //    //{
        //    //    Debug.Log($"y-angle: {transform.eulerAngles.y}");
        //    //    transform.Rotate(0, 0.09f, 0);
        //    //    yield return null;
        //    //}

        //    transform.rotation = Quaternion.Euler(0, 180, 0);
        //    finishedTaxing = true;
        //}

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

        IEnumerator WaitForAnyCondition(Func<bool> conditionMethod, Action actionMethod = null)
        {
            while (!conditionMethod())
            {
                yield return null;
            }

            actionMethod?.Invoke();
        }

        public IEnumerator RotateRotors()
        {
            if (aircraftRotors.Count() > 0)
            {
                while (true)
                {
                    foreach (var rotor in aircraftRotors)
                    {
                        rotor.transform.Rotate(rotorSpeed * Time.deltaTime * Vector3.forward);
                    }
                    yield return null;
                }
            }
            
        }

        public void DestroyAircraft()
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }

    }
}
