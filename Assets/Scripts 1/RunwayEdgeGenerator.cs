using UnityEngine;

public class RunwayEdgeGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject runwayLightPrefab;
    [SerializeField] private Transform runwayLightParent;

    [Header("Runway")]
    public Transform startingPoint;
    public Transform endPoint;

    public  float runwayWidth = 22.0f;
    private float lightIntervals = 30.0f;

    public void Generate() 
    {
        Clear();
        Vector3 direction = (endPoint.position - startingPoint.position).normalized;
        float length = Vector3.Distance(startingPoint.position, endPoint.position);
        Vector3 right = Vector3.Cross(Vector3.up, direction);
        int lightCount = Mathf.FloorToInt(length / lightIntervals);

        for(int l =0; l <= lightCount; l++) 
        {
            Vector3 rightPos = startingPoint.position + direction * (l * lightIntervals);
            Vector3 leftPos = rightPos - right * runwayWidth;
            Instantiate(runwayLightPrefab, rightPos, Quaternion.identity, runwayLightParent);
            Instantiate(runwayLightPrefab, leftPos, Quaternion.identity, runwayLightParent);
        }
    }

    public void Clear() 
    {
        while(runwayLightParent.childCount > 0) 
        {
#if UNITY_EDITOR
            DestroyImmediate(runwayLightParent.GetChild(0).gameObject);
#else
        Destroy(runwayLightsParent.GetChild(0).gameObject);
#endif
        }
    }
}
