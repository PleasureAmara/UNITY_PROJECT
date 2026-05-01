using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace localizer.product.vehicle
{
    public class NavigateCar : MonoBehaviour
    {
        [SerializeField] private float maxForwardSpeed = 15.0f;
        [SerializeField] private float minForwardSpeed = 5.0f;

        [HideInInspector] public bool hasCarReached;

        void Start()
        {

        }

        void Update()
        {
            //transform.Translate(forwardSpeed * Time.deltaTime * Vector3.back, Space.Self);
        }

        float carSpeed = 0;
        bool hasCarReachedDecelerationPoint;
        bool hasCarReachedStoppingPoint;
        public IEnumerator MoveCarForward(float stopPositionZ)
        {
            hasCarReached = false;
            hasCarReachedDecelerationPoint = false;
            hasCarReachedStoppingPoint = false;

            float startPositionZ = transform.position.z;
            float divisor = Mathf.Abs(stopPositionZ - startPositionZ) * 1 / 8;
            float deceleratingPositionZ = (divisor * 6) + startPositionZ;
            float stoppingPositionZ = divisor * 7 + startPositionZ;
            Vector3 deceleratingPosition = new Vector3(transform.position.x, transform.position.y, deceleratingPositionZ);
            Vector3 stoppingPosition = new Vector3(transform.position.x, transform.position.y, stoppingPositionZ);
            Vector3 finalPosition = new Vector3(transform.position.x, transform.position.y, stopPositionZ);
            Debug.Log($"decelration position: {deceleratingPosition}");
            //float movtDirection = Mathf.Sign(stopPosition - startPosition);
            //Vector3 movtDirection = (transform.position - new Vector3(transform.position.x, transform.position.y, stopPosition) ).normalized;

            
            //float acceleration = 0.1f;
            StartCoroutine(AdjustCarSpeed());
            while (Vector3.Distance(transform.position, deceleratingPosition) > 0.05f)
                //while (Mathf.Abs(stopPositionZ-transform.position.z) > 0.05)
            {
                //transform.Translate(carSpeed * Time.deltaTime * movtDirection, Space.Self);
                //if (carSpeed < maxForwardSpeed)
                //{
                //    carSpeed += acceleration;
                //}


                transform.position = Vector3.MoveTowards(transform.position, deceleratingPosition, carSpeed * Time.deltaTime);
                Debug.Log($"Current Car position: {transform.position} and speed: {carSpeed}");
                yield return null;
            }

            hasCarReachedDecelerationPoint = true;
            while (Vector3.Distance(transform.position, stoppingPosition) > 0.05f)
            {
                //transform.Translate(carSpeed * Time.deltaTime * movtDirection, Space.Self);
                //if (carSpeed > minForwardSpeed)
                //{
                //    carSpeed -= acceleration;
                //}
                transform.position = Vector3.MoveTowards(transform.position, stoppingPosition, carSpeed * Time.deltaTime);
                Debug.Log($"Current Car position: {transform.position} and speed: {carSpeed}");
                yield return null;
                
            }

            hasCarReachedStoppingPoint = true;
            while (Vector3.Distance(transform.position, finalPosition) > 0.05f)
            {
                //transform.Translate(carSpeed * Time.deltaTime * movtDirection, Space.Self);
                //if (carSpeed > minForwardSpeed)
                //{
                //    carSpeed -= acceleration;
                //}
                transform.position = Vector3.MoveTowards(transform.position, finalPosition, carSpeed * Time.deltaTime);
                Debug.Log($"Current Car position: {transform.position} and speed: {carSpeed}");
                yield return null;

            }

            hasCarReached = true;
        }

        IEnumerator AdjustCarSpeed()
        {
            //float carSpeed = 0;
            float acceleration = 0.5f;
            float deceleration = 1.0f;
            float finalDeceleration = 0.5f;

            while (carSpeed < maxForwardSpeed)
            {
                carSpeed += acceleration;
                yield return new WaitForSeconds(1);
            }

            while (carSpeed> minForwardSpeed)
            {
                if (hasCarReachedDecelerationPoint)
                {
                    carSpeed -= deceleration;
                }
                yield return new WaitForSeconds(1);

            }

            while (carSpeed > 2)
            {
                if (hasCarReachedStoppingPoint)
                {
                    carSpeed -= finalDeceleration;
                }
                yield return new WaitForSeconds(1);

            }
        }
    }
}
