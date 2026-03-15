using System.Collections;
using UnityEngine;

namespace localizer.product.environment
{
    public class BirdManager : MonoBehaviour
    {
        [SerializeField] private AudioSource[] birdSounds;

        [Tooltip("Drag the Sound Controller component attached to this game object.")]
        [SerializeField] private SoundController soundController;


        private void Start()
        {
            StartBirds();
        }

        /// <summary>
        /// Start all the birds provided in the array birdSounds under the component Bird Manager.
        /// </summary>
        public void StartBirds()
        {
            if (birdSounds == null) return;

            int delay = 0;
            foreach (var birdSound in birdSounds)
            {
                StartCoroutine(PlaySounds(birdSound, delay));
                delay += 5;
            }
        }

        public void StopAllBirds()
        {
            StopAllCoroutines();
        }

        IEnumerator PlaySounds(AudioSource sound, int soundDelay)
        {
            while (true)
            {
                sound.PlayDelayed(soundDelay);
                                        yield return null;
            }
        }
    }
}

