using UnityEngine;

public class HighlightController : MonoBehaviour
{
    private MeshRenderer rend;
    private MaterialPropertyBlock propBlock;

    public float glowOnValue = 150f;
    public float glowOffValue = 0f;

    //private void Awake()
    //{
    //    propBlock = new MaterialPropertyBlock();
    //    rend = GetComponent<MeshRenderer>();
    //    if (rend == null)
    //    {
    //        Debug.LogError($"HighlightController on '{gameObject.name}' could not find a MeshRenderer.");
    //    }

    //}

    private void Awake()
    {
        propBlock = new MaterialPropertyBlock();

        rend = GetComponentInChildren<MeshRenderer>();
    }

    public void SetHighlight(bool active)
    {
        if (rend == null) return;

        rend.GetPropertyBlock(propBlock);
        propBlock.SetFloat("_GlowStrength", active ? glowOnValue : glowOffValue);
        rend.SetPropertyBlock(propBlock);
    }


}


