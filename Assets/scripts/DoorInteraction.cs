using System;
using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
public class DoorInteraction : MonoBehaviour
{
    public static event Action<DoorInteraction> DoorOpened;

    [Header("Door Settings")]
    public float openAngleThreshold = 30f; // degrees

    private HingeJoint hinge;
    private bool hasFired = false;

    private void Awake()
    {
        hinge = GetComponent<HingeJoint>();
    }

    private void Update()
    {
        float angle = Mathf.Abs(hinge.angle);

        if (!hasFired && angle >= openAngleThreshold)
        {
            hasFired = true;
            DoorOpened?.Invoke(this);
        }
    }
}