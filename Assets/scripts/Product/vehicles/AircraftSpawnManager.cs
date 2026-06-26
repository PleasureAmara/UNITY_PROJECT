using localizer.product.airplane;
using localizer.product.player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace localizer.product.vehicle
{
    public class AircraftSpawnManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> aircrafts;

        //we use this instance to track when the introduction has been finished.
        [SerializeField] private IntroductionManager introductionManager;
        //private Vector3 spawnPosition = new Vector3(1302, 50.73f, -70);
        private readonly float periodBeforeTakeOff = 3.0f;
        private readonly float periodBeforeNewSpawn = 10.0f;

        //UNCOMMENT
        //private void OnEnable()
        //{
        //    introductionManager.isIntroFinished += StartSpawn;
        //}

        //private void OnDisable()
        //{
        //    introductionManager.isIntroFinished -= StartSpawn;
        //}

        private void Start()
        {
            StartCoroutine(SpawnAircraft());
        }

        public IEnumerator SpawnAircraft()
        {
            if (aircrafts.Count == 0) yield break;

            GameObject chosenAircraft = aircrafts[0];
            AirplaneTaxi taxiScript = chosenAircraft.GetComponent<AirplaneTaxi>();
            AirplaneTakeOff takeOffScript = chosenAircraft.GetComponent<AirplaneTakeOff>();

            StartCoroutine(taxiScript.RotateRotors());
            taxiScript.StartTaxi();
            while (!taxiScript.hasFinishedTaxing)
            {
                yield return null;
            }
            yield return new WaitForSeconds(periodBeforeTakeOff);
            takeOffScript.StartTakeOff();

            while (takeOffScript.isAircraftVisual)
            {
                yield return null;
            }
            StopAllCoroutines();
            takeOffScript.DestroyAircraft();
            
            //remove the aircraft from the list
            aircrafts.RemoveAt(0);

            yield return new WaitForSeconds(periodBeforeNewSpawn);

            StartCoroutine(SpawnAircraft());
        }
    }
}