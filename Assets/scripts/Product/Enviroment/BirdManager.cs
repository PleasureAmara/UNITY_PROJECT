using System.Collections;
using UnityEngine;

namespace localizer.product.environment
{
    public class BirdManager : MonoBehaviour
    {
        [SerializeField] private AudioSource[] birdSounds;

        [Tooltip("Drag the Sound Controller component attached to this game object.")]
        [SerializeField] private SoundController soundController;

        /// <summary>
        /// the time between the birds sound repetitions.
        /// </summary>
        private float chirppingDelay = 2.0f;
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
            foreach (var birdSound in birdSounds)
            {
                StartCoroutine(PlaySounds(birdSound, chirppingDelay) );
            }
        }

        public void StopAllBirds()
        {
            StopAllCoroutines();
        }

        IEnumerator PlaySounds(AudioSource sound, float soundDelay)
        {
            while (true)
            {
                sound.Play();
                yield return new WaitForSeconds(soundDelay);
            }
        }
    }
}

