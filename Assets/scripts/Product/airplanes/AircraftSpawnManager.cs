using localizer.product.airplane;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace localizer.product.airplane
{
    public class AircraftSpawnManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] aircfraftsToSpawn;
        private float spawnDelay = 5.0f;
        private Vector3 startingPosition = new Vector3(1288, 50.776f, -67);
        [SerializeField] private GameObject targetPosition;

        void Start()
        {
            StartCoroutine(SpawnAircraft());
        }

        IEnumerator SpawnAircraft()
        {
            while (true)
            {
                GameObject chosenPrefab = aircfraftsToSpawn[Random.Range(0, aircfraftsToSpawn.Length)];
                GameObject chosenAircraft =  Instantiate(chosenPrefab, startingPosition, chosenPrefab.transform.rotation);
                chosenAircraft.transform.SetParent(null, false);
                chosenAircraft.transform.localScale = Vector3.one;
                chosenAircraft.name = "Boeing";
                //GameObject chosenAircraft = Instantiate(chosenPrefab, targetPosition.transform.position, targetPosition.transform.rotation);

                Debug.Log($"Spawned: {chosenAircraft.name}");
                Debug.Log($"Position: {chosenAircraft.transform.position}");
                Debug.Log($"Scale: {chosenAircraft.transform.localScale}");
                Debug.Log($"Active: {chosenAircraft.activeSelf}");
                Debug.Log($"Renderer: {chosenAircraft.GetComponentInChildren<Renderer>()}");


                AirplaneTaxi taxiScript = chosenAircraft.GetComponent<AirplaneTaxi>();
                AirplaneTakeOff takeoffScript = chosenAircraft.GetComponent<AirplaneTakeOff>();
                
                //we use timers to provide a safe guard incase the booleans in the while loops dont change state, if that happens causes
                //coroutines to run forever causing performance degradation and unintended performance. 
                float timeOut = 30.0f;
                float timer = 0.0f;


                while (!taxiScript.finishedTaxing)
                {
                    Debug.Log("Äircraft taxing");
                    timer += Time.deltaTime;    
                    yield return null;
                }
                //takeoffScript.StartTakeOff();

                //timer = 0;
                //while (!takeoffScript.isAircraftVisual && timer < timeOut)
                //{
                //    timer += Time.deltaTime;
                //    yield return null;
                //}
                //takeoffScript.DestroyAircraft();

                //yield return new WaitForSeconds(spawnDelay);
            }
           
        }
    }
}
