using UnityEngine;

public class HideAvatarHead : MonoBehaviour
{
    [SerializeField] Animator animator;//must be set to humanoid rig type on import
    Transform headBone;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Using unity's humanoid mapping system instead of a hard-coded name which keeps it working even if you later swap a different mixamo character
        headBone = animator.GetBoneTransform(HumanBodyBones.Head);

    }

    // Update is called once per frame
    void LateUpdate()//Guarantees the scaling happens after the Animator applies its transforms
    {
        if(headBone != null) 
        {
            headBone.localScale = Vector3.one * 0.001f; //scales head bone to near zero but not exact zero
        }
    }
}
