using UnityEngine;

public class RunwayLights : MonoBehaviour
{
    [SerializeField] Light pointLight;
    [SerializeField] Renderer glowRenderer;
    MaterialPropertyBlock block;

    void Awake()
    {
        block = new MaterialPropertyBlock();
    }
    void OnEnable()
    {
        AirportLightingManager.Instance.RegisterRunwayLight(this);
    }

    void OnDisable()
    {
        AirportLightingManager.Instance.UnRegisterRunwayLight(this);
    }

    public void ApplySettings(float intensity, float emission)
    {
        pointLight.intensity = intensity;
        glowRenderer.GetPropertyBlock(block);
        block.SetColor("_EmissionColor", Color.white * emission);
        glowRenderer.SetPropertyBlock(block);
    }
}
