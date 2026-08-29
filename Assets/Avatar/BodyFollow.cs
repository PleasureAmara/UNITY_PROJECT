using UnityEngine;

public class BodyFollow : MonoBehaviour
{
    [SerializeField] Transform xrCamera; //VR Heaset camera transform that the body will follow
    [SerializeField] Animator animator; //animator component on the body that controls animations
    [SerializeField] LayerMask groundMask = ~0; //set to floor/terrain layers and not everything
    [SerializeField] float eyeToFloorFallBack = 1.5f; //used if raycast finds no ground
    [SerializeField] float speedSmoothTime = 0.15f;//how smoothly the animation speed transitions

    Vector2 lastFlatPosition;
    float speedVelocity;
    void Start()
    {
       lastFlatPosition = new Vector2(xrCamera.position.x, xrCamera.position.z);
    }
    void LateUpdate()
    {
        //obtains the instanteneous speed by calculating the Camera's current position from its previous position.
        Vector3 cameraPosition = xrCamera.position;

        float groundY;
        if (Physics.Raycast(cameraPosition + Vector3.up * 2f, Vector3.down, out RaycastHit hit, 10f, groundMask))// fires an invisible laser beam (a raycast) to detect the floor
        {
            groundY = hit.point.y;//if the ray successfully hits a groundMask collider, the y-value(height) is extracted and stored
        }
        else
        {
            groundY = cameraPosition.y - eyeToFloorFallBack; // no ground hit, estimate instead
        }

        //horizontal speed only, so a ramp/step does not feed a vertical jump into the walk blend
        Vector2 currentFlatPosition = new Vector2(cameraPosition.x, cameraPosition.z);

        float instSpeed = (currentFlatPosition - lastFlatPosition).magnitude / Time.deltaTime;
        lastFlatPosition = currentFlatPosition;

        transform.position = new Vector3(cameraPosition.x, groundY, cameraPosition.z);
        Vector3 fwd = xrCamera.forward; fwd.y = 0f; //rotate player Horizontally(yaw), not up/ down(pitch) or tilt(roll) I
        if (fwd.sqrMagnitude > 0.001f)
        {

            //It extracts the camera's forward direction, removes the Y component, and uses Quaternion.LookRotation() to face that direction
            transform.rotation = Quaternion.LookRotation(fwd);
        }

        //smoothly transition the animation speed parameter, preventing sudden jumps
        float smoothed = Mathf.SmoothDamp(animator.GetFloat("Speed"), instSpeed, ref speedVelocity, speedSmoothTime);
        animator.SetFloat("Speed", smoothed);

    }

    
}
