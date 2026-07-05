using UnityEngine;
using System.Collections;
using localizer.core.interfaces;
using UnityEngine.UIElements;

namespace localizer.product.vehicle
{
    public class NavigateVehicle : MonoBehaviour
    {
        //[SerializeField] private TaxiRwyPivot taxiRwyPivot;

        [Header("Speed values")]
        [SerializeField] public  float maxForwardSpeed = 15.0f;
        [SerializeField] private float minForwardSpeed = 5.0f;
        [SerializeField] private float stoppingSpeed = 3.0f; 
        [SerializeField] private bool doesVehicleHaveSteering;
        [Tooltip("Drag the steering pivot of your vehicle. This only works if you have enabled the above boolean doesVehicleHaveSteering")]
        [SerializeField] private GameObject steeringPivot;

        [HideInInspector] public bool hasVehicleReached;

        [HideInInspector] public float vehicleSpeed = 0f;
        [HideInInspector] public readonly float acceleration = 0.5f;
        float deceleration = 1.0f;
        float finalDeceleration = 0.5f;

        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stopPosition"></param>
        /// <param name="movementAxis"> the axis of movement to the final position, i.e x or z </param>
        /// <returns></returns>
        public IEnumerator MoveVehicleForward(Vector3 stopPosition, BasePivot rotationPivot = null)
        {
            
            Vector3 startPosition = transform.position;
            float distanceBtnStopNStart = Vector3.Distance(startPosition, stopPosition);
            float distanceToDeceleratePosition = 0.75f * distanceBtnStopNStart; 
            float distanceToStoppingPosition = 0.875f * distanceBtnStopNStart;

            //Debug.Log($"Start position: {startPosition}");
            //Debug.Log($"stop position passed as parameter: {stopPosition}");
            //Debug.Log($"Distance Start n stop position: {distanceBtnStopNStart}");

            hasVehicleReached = false;

            ///##################THE MATH:###########################
            // start                                                decelerating                stopping            stop
            // point                                                   point                     point              point
            // |<-------------------------------------a--------------------------------------------------------------->|
            // |<-------------------------------------b = (7/8) * a-------------------------------->|
            // |<------------------------------c = (6/8) * a ----------->|
            //                  |<--------------------d--------------------------------------------------------------->|
            // |<----e=(a-d)--->|
            // 
            //  d -> the instanteneous distance of the vehicle to the stop point.
            //  e -> the instanteneous distance of vehicle from start point
            //  depending on the target e.g stopping point, we check if the distance 'e' < 'b' for each movement.
            // this is what's implemented in the while loops below.


            Coroutine AccelerateCoroutine = StartCoroutine(AccelerateVehicle(maxForwardSpeed, acceleration));
            while (Mathf.Abs(distanceBtnStopNStart- Vector3.Distance(transform.position, stopPosition)) < distanceToDeceleratePosition)
            {
                //Debug.Log($"Vehicle speed: {vehicleSpeed}");
                transform.position = Vector3.MoveTowards(transform.position, stopPosition, vehicleSpeed * Time.deltaTime);
                //Debug.Log($"Vehicle position: {transform.position}");
                yield return null;
            }

            //StopCoroutine(AccelerateCoroutine);
            StopAllCoroutines();
            Coroutine decelerateCoroutine = StartCoroutine(DecelerateVehicle(minForwardSpeed, deceleration));
            while (Mathf.Abs(distanceBtnStopNStart - Vector3.Distance(transform.position, stopPosition)) < distanceToStoppingPosition)
            {
                //Debug.Log($"Vehicle speed: {vehicleSpeed}");
                transform.position = Vector3.MoveTowards(transform.position, stopPosition, vehicleSpeed * Time.deltaTime);
                //Debug.Log($"Vehicle position: {transform.position}");
                yield return null;
            }

            //StopCoroutine(decelerateCoroutine);
            StopAllCoroutines();
            Coroutine stopCoroutine = StartCoroutine(DecelerateVehicle(stoppingSpeed, finalDeceleration));
            while (Mathf.Abs(distanceBtnStopNStart - Vector3.Distance(transform.position, stopPosition)) < distanceBtnStopNStart)
            {
                
                transform.position = Vector3.MoveTowards(transform.position, stopPosition, vehicleSpeed * Time.deltaTime);
                //Debug.Log($"Vehicle position: {transform.position}");
                yield return null;
            }

            //in some situations moving forward may include turning too at the end of the straight line motion, thus the
            //condition below ensure satisfaction of that scenario.
            if (rotationPivot != null)
            {
                TurnVehicle(rotationPivot);
                //wait for turning to finish before you set the boolean hasVehicleReached to true. 
                while (!hasFinishedTurning)
                {
                    yield return null;
                }
            }
            hasVehicleReached = true;
        }

        public IEnumerator AccelerateVehicle(float maxSpeed, float accelerationValue)
        {
            //Debug.Log($"max forward speed: {maxSpeed}");
            while (vehicleSpeed < maxSpeed)
            {
                vehicleSpeed += accelerationValue;
                yield return new WaitForSeconds(1);
            }
        }

        IEnumerator DecelerateVehicle(float minSpeed, float decelerationValue)
        {
            while (vehicleSpeed > minSpeed)
            {
                vehicleSpeed -= decelerationValue;
                yield return new WaitForSeconds(1);
            }
        }

        [HideInInspector] public bool hasFinishedTurning;
        public void TurnVehicle(BasePivot pivot)
        {
            pivot.attachedVehicleScript = this;
            if (doesVehicleHaveSteering && steeringPivot != null)
            {
                StartCoroutine(pivot.RotatePivot(steeringPivot));
                return;
            }
            StartCoroutine(pivot.RotatePivot());
        }
    }
}

