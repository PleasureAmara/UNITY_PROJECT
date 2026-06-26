using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(XRGrabInteractable))]
public class DoorMovement : MonoBehaviour
{
    private Rigidbody rb;
    private XRGrabInteractable grab;

    [Header("Freeze Settings")]
    public bool freezeOnRelease = true;
    public bool disableGravityWhenFrozen = true;

    private bool isFrozen = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        grab = GetComponent<XRGrabInteractable>();

        grab.selectExited.AddListener(OnRelease);
        grab.selectEntered.AddListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // Unfreeze when grabbed again
        if (isFrozen)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            isFrozen = false;
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (transform.localEulerAngles.y > 50f)
        {
            // freeze logic

            if (!freezeOnRelease) return;

            // Stop all motion instantly
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            // Freeze in place
            rb.angularVelocity = Vector3.zero;

            if (disableGravityWhenFrozen)
                rb.useGravity = false;

            isFrozen = true;
        }
    }
}