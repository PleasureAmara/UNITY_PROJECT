using System;
using UnityEngine;

public class ProximityTrigger : MonoBehaviour
{ 

    public static event Action<ProximityTrigger> PlayerEntered;
    public static event Action<ProximityTrigger> PlayerExited;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
            PlayerEntered?.Invoke(this);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            PlayerExited?.Invoke(this);
        }
    }

}


