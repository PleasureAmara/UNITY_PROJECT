using localizer.product.airplane;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace localizer.product.vehicle
{
    public class AircraftSpawnManager : MonoBehaviour
    {
        public GameObject[] aircrafts;
        private Vector3 spawnPosition = new Vector3(1302, 50.73f, -70);
        private float periodBeforeTakeOff = 3.0f;
        private float periodBeforeNewSpawn = 5.0f;


        //private void Start()
        //{
        //    StartCoroutine(SpawnAircraft());
        //}

        IEnumerator SpawnAircraft()
        {
            GameObject chosenPrefab = aircrafts[Random.Range(0, aircrafts.Length)];
            GameObject chosenAircraft = Instantiate(chosenPrefab, spawnPosition, chosenPrefab.transform.rotation);

            AirplaneTaxi taxiScript = chosenAircraft.GetComponent<AirplaneTaxi>();
            AirplaneTakeOff takeOffScript = chosenAircraft.GetComponent<AirplaneTakeOff>();

            taxiScript.StartTaxi();
            while (!taxiScript.finishedTaxing)
            {
                yield return null;
            }
            yield return new WaitForSeconds(periodBeforeTakeOff);
            takeOffScript.StartTakeOff();

            while (takeOffScript.isAircraftVisual)
            {
                yield return null;
            }
            takeOffScript.DestroyAircraft();

            yield return new WaitForSeconds(periodBeforeNewSpawn);

            StartCoroutine(SpawnAircraft());
        }
    }
}