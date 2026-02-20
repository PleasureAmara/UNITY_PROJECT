using localizer.core.interfaces;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Rendering;

public class RotateKey : MonoBehaviour, IClickable
{
    [SerializeField] private GameObject normalObject;
    [SerializeField] private GameObject warningObject;
    [SerializeField] private GameObject alarmObject;

    private Material normalColorMaterial;
    private Material warningColorMaterial;
    private Material alarmColorMaterial;

    //private Color defaultColor = Color.white;
    //private Color normalColor = Color.red;
    //private Color warningColor = Color.orange;
    //private Color alarmColor = Color.green;

    private int triggerPressCount = 0;
    //private bool triggerHeld = false;

    void Start()
    {
        normalColorMaterial = normalObject.GetComponent<MeshRenderer>().material;
        warningColorMaterial = warningObject.GetComponent<MeshRenderer>().material;
        alarmColorMaterial = alarmObject.GetComponent<MeshRenderer>().material;

        ControlEquipmentState();
    }


    public void ClickAction()
    {
        SetKeyPosition();
        Debug.Log($"The z position: {transform.position.z}");
        ControlEquipmentState();

    }

    private void ControlEquipmentState()
    {
        normalColorMaterial.DisableKeyword("_EMISSION");
        warningColorMaterial.DisableKeyword("_EMISSION");
        alarmColorMaterial.DisableKeyword("_EMISSION");

        switch (triggerPressCount)
        {
            case 0:
                Debug.Log("Normal is shining");
                normalColorMaterial.EnableKeyword("_EMISSION");
                break;

            case 1:
                Debug.Log("Warning is shining");
                warningColorMaterial.EnableKeyword("_EMISSION");
                break;

            case 2:
                Debug.Log("Alarm is shining");
                alarmColorMaterial.EnableKeyword("_EMISSION");
                break;
        }
    }

    private void SetKeyPosition()
    {
        triggerPressCount++;

        if (triggerPressCount > 2)
        {
            triggerPressCount = 0;
        }
        

        switch (triggerPressCount)
        {
            case 0:
                transform.rotation = Quaternion.Euler(0,0,0);
                break;
            case 1:
                transform.rotation = Quaternion.Euler(0, 0, -45);
                break;
            case 2:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
        }
        

        // this method of incrementing brings errors since unity adds decimals thus shifting the increment from 
        ////Exact positions
        //if (transform.rotation.z > -90)
        //{
        //    transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z -45));
        //    return;
        //}
        //transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, 0));

    }
}
