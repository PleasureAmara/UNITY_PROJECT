using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashMove : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference leftMoveAction;

    [Header("References")]
    public CharacterController characterController;
    public Transform headTransform;

    [Header("Dash Settings")]
    public float dashDistance = 1.25f;
    public float dashDuration = 0.12f;
    public float cooldown = 0.2f;
    public float triggerThreshold = 0.75f;
    public float resetThreshold = 0.25f;
    public bool cardinalDirectionsOnly = true;

    private bool dashReady = true;
    private bool isDashing;
    private float nextDashTime;

    private void Awake()
    {
        if (characterController == null)
            characterController = GetComponent<CharacterController>();

        if (headTransform == null && Camera.main != null)
            headTransform = Camera.main.transform;
    }

    private void Update()
    {
        Vector2 input = ReadInput(leftMoveAction);

        if (input.magnitude <= resetThreshold) 
        {
            dashReady = true;
            return;
        }

        if (!dashReady || isDashing || Time.time < nextDashTime)
            return;

        if (input.magnitude < triggerThreshold)
            return;

        dashReady = false;

        Vector3 direction = GetDashDirection(input);

        if (direction.sqrMagnitude > 0.001f)
            StartCoroutine(Dash(direction.normalized));


    }

    private Vector2 ReadInput(InputActionReference actionReference) 
    {
        if (actionReference == null || actionReference.action == null)
            return Vector2.zero;

        return actionReference.action.ReadValue<Vector2>();
    }
    private Vector3 GetDashDirection(Vector2 input)
    {
        if (cardinalDirectionsOnly)
        {
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                input = new Vector2(Mathf.Sign(input.x), 0f);
            else
                input = new Vector2(0f, Mathf.Sign(input.y));
        }

        Transform reference = headTransform != null ? headTransform : transform;

        Vector3 forward = Vector3.ProjectOnPlane(reference.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(reference.right, Vector3.up).normalized;

        return forward * input.y + right * input.x;
    }

    private IEnumerator Dash(Vector3 direction)
    {
        isDashing = true;

        float elapsed = 0f;
        float speed = dashDistance / dashDuration;

        while (elapsed < dashDuration)
        {
            float stepTime = Mathf.Min(Time.deltaTime, dashDuration - elapsed);
            Vector3 movement = direction * speed * stepTime;

            if (characterController != null && characterController.enabled)
                characterController.Move(movement);
            else
                transform.position += movement;

            elapsed += stepTime;
            yield return null;
        }

        nextDashTime = Time.time + cooldown;
        isDashing = false;
    }






}
