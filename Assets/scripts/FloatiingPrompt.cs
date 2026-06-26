using UnityEngine;
using TMPro;
public class FloatiingPrompt : MonoBehaviour
{
    public Transform target; // The object to follow
    float offset = 0.3f; // Offset from the target position
    public TextMeshProUGUI promptText;
   

    private Transform playerCamera;
    void Start()
    {
        playerCamera = Camera.main.transform; // Get the main camera's transform
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 toCamera = (playerCamera.transform.position - target.position).normalized;

            transform.position = target.position + toCamera*offset; // Follow the target with the specified offset
        }

        if (playerCamera != null)
        {
            transform.LookAt(playerCamera); // Make the prompt face the camera
            transform.Rotate(0, 180, 0); // Rotate the prompt to face the camera correctly
        }
    }
    public void SetText(string message)
    {
        promptText.text = message;
    }

    public void Initialize(Transform targetTransform, string message)
    {
        target = targetTransform;
        promptText.text = message;
    }

}

