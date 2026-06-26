using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;



public class alarmColor : MonoBehaviour
{
    [Header("XR Trigger Action")]
    public InputActionReference trigAction;

    [Header("Colors")]
    public Color defaultColor = Color.white;
    public Color triggerColor = Color.orange;
    private MeshRenderer Rend;

    void Awake()
    {
        Rend = GetComponent<MeshRenderer>();
        Rend.material.color = defaultColor;
    }

    void OnEnable()
    {
        trigAction.action.Enable();
    }

    void OnDisable()
    {
        trigAction.action.Disable();
    }

    void Update()
    {
        if (trigAction.action.IsPressed())
        {
            Rend.material.color = triggerColor;
        }
        else
        {
            Rend.material.color = defaultColor;
        }

    }

    //private MeshRenderer Rend;
    //private Color defaultColor = Color.white;
    //private Color trigColor = Color.orange;

    //void Start()
    //{
    //    Rend = GetComponent<MeshRenderer>();
    //    Rend.material.color = defaultColor;
    //}

    //private void Update()
    //{
    //    if (Input.GetKey(KeyCode.Space))
    //    {
    //        Rend.material.color = trigColor;
    //    }
    //    else
    //    {
    //        Rend.material.color = defaultColor;
    //    }
    //}

}
