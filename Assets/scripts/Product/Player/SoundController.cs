using System.Security.Cryptography;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public void PlaySound(AudioSource audio, float delayTime)
    {
        audio.PlayDelayed(delayTime);

    }

    public void PlaySound(AudioSource audio)
    {
        audio.Play();
    }
}
