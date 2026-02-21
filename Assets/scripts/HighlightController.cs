using UnityEngine;

public class HighlightController : MonoBehaviour
{

    private MeshRenderer rend;
    private MaterialPropertyBlock propBlock;

    public float glowOnValue = 150f;
    public float glowOffValue = 0f;

    private void Awake()
    {
        propBlock = new MaterialPropertyBlock();
        if (rend == null)
            rend = GetComponent<MeshRenderer>();
        
    }

    public void SetHighlight(bool active)
    {
        rend.GetPropertyBlock(propBlock);
        propBlock.SetFloat("_GlowStrength", active ? glowOnValue : glowOffValue);
        rend.SetPropertyBlock(propBlock);
    }
}


