using UnityEngine;

public class NavigateCar : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 7.0f;
    
    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(forwardSpeed * Time.deltaTime * Vector3.back, Space.Self);
    }
}
