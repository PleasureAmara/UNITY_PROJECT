using UnityEngine;
using System.Collections;
using localizer.core.interfaces;

namespace localizer.product.vehicle
{
    public class NavigateVehicle : MonoBehaviour
    {
        //[SerializeField] private TaxiRwyPivot taxiRwyPivot;

        [Header("Speed values")]
        [SerializeField] private float maxForwardSpeed = 15.0f;
        [SerializeField] private float minForwardSpeed = 5.0f;
        [SerializeField] private float stoppingSpeed = 3.0f;

        [HideInInspector] public bool hasVehicleReached;

        float vehicleSpeed = 0f;
        float acceleration = 0.5f;
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
        public IEnumerator MoveVehicleForward(float stopPositionZ)
        {
            //string movtAxis = movementAxis.ToLower();
            float startPositionZ = transform.position.z;
            float divisor = (stopPositionZ - startPositionZ) * 1 / 8;
            float deceleratingPositionZ = (divisor * 6) + startPositionZ;
            float stoppingPositionZ = divisor * 7 + startPositionZ;

            //switch (movementAxis.ToLower())
            //{
            //    case "z":

            //        break;
            //}
            hasVehicleReached = false;

            Vector3 deceleratingPosition = new Vector3(transform.position.x, transform.position.y, deceleratingPositionZ);
            Vector3 stoppingPosition = new Vector3(transform.position.x, transform.position.y, stoppingPositionZ);
            Vector3 finalPosition = new Vector3(transform.position.x, transform.position.y, stopPositionZ);

            Vector3 direction = (deceleratingPosition - transform.position).normalized;
            Coroutine AccelerateCoroutine = StartCoroutine(AccelerateVehicle(maxForwardSpeed, acceleration));
            while (Vector3.Distance(transform.position, deceleratingPosition) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, deceleratingPosition, vehicleSpeed * Time.deltaTime);
                yield return null;
                //rb.linearVelocity = transform.forward * vehicleSpeed ;
                //yield return new WaitForFixedUpdate();
            }

            StopCoroutine(AccelerateCoroutine);
            Coroutine decelerateCoroutine = StartCoroutine(DecelerateVehicle(minForwardSpeed, deceleration));
            while (Vector3.Distance(transform.position, stoppingPosition) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, stoppingPosition, vehicleSpeed * Time.deltaTime);
                //Debug.Log($"Current Car position: {transform.position} and speed: {vehicleSpeed}");
                yield return null;
                //rb.linearVelocity = transform.forward * vehicleSpeed ;
                //yield return new WaitForFixedUpdate();
            }

            StopCoroutine(decelerateCoroutine);
            Coroutine stopCoroutine = StartCoroutine(DecelerateVehicle(stoppingSpeed, finalDeceleration));
            while (Vector3.Distance(transform.position, finalPosition) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, finalPosition, vehicleSpeed * Time.deltaTime);
                //Debug.Log($"Current Car position: {transform.position} and speed: {vehicleSpeed}");
                yield return null;
                //rb.linearVelocity = transform.forward * vehicleSpeed;
                //yield return new WaitForFixedUpdate();
            }

            hasVehicleReached = true;
        }

        IEnumerator AccelerateVehicle(float maxSpeed, float accelerationValue)
        {
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
            StartCoroutine(pivot.RotatePivot());
            return;
            //Debug.LogError("The gameobject containing taxiRwyPivot is missing, Drag it on the gameobject having NavigateVehicle component.");
        }
    }
}

