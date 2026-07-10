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

        private readonly float periodBeforeTakeOff = 3.0f;
        private readonly float periodBeforeNewSpawn = 10.0f;

        private void OnEnable() => introductionManager.isIntroFinished += StartSpawn;
        private void OnDisable() => introductionManager.isIntroFinished -= StartSpawn;


        private void StartSpawn()
        {
            StartCoroutine(SpawnAircraft());
        }

        public IEnumerator SpawnAircraft()
        {
            while (aircrafts.Count > 0)
            { 
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

            //remove the aircraft from the list
            aircrafts.RemoveAt(0);
            takeOffScript.DestroyAircraft();

            yield return new WaitForSeconds(periodBeforeNewSpawn);

            }
        }
    }
}