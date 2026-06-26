using UnityEngine;
using UnityEngine.InputSystem;

public class ColorChange : MonoBehaviour
{
    public InputActionReference triggerAction;

    public MeshRenderer cubeOne;
    public MeshRenderer cubeTwo;
    public MeshRenderer cubeThree;


    public Color defaultColor = Color.white;
    public Color target1 = Color.red;
    public Color target2 = Color.orange;
    public Color target3 = Color.green;

    private int triggerPressCount = 0;
    private bool triggerHeld = false;

    void Awake()
    {
        cubeOne = GameObject.Find("Normal").GetComponent<MeshRenderer>();
        cubeTwo = GameObject.Find("Warning").GetComponent<MeshRenderer>();
        cubeThree = GameObject.Find("Alarm").GetComponent<MeshRenderer>();

        cubeOne.material.color = defaultColor;
        cubeTwo.material.color = defaultColor;
        cubeThree.material.color = defaultColor;
    }
    void OnEnable()
    {
        triggerAction.action.Enable();
    }

    void OnDisable()
    {
        triggerAction.action.Disable();
    }

    void Update()
    {
        if (triggerAction.action.IsPressed())
        {
            if (!triggerHeld)
            {
                triggerHeld = true;
                HandleTriggerPress();
            }
        }
        else
        {
            triggerHeld = false;
        }
        
    }

    private void HandleTriggerPress()
    {
        triggerPressCount++;

        int index = (triggerPressCount - 1) % 3;

        // Reset all cubes first
        cubeOne.material.color = defaultColor;
        cubeTwo.material.color = defaultColor;
        cubeThree.material.color = defaultColor;

        // Activate only one cube
        switch (index)
        {
            case 0:
                cubeOne.material.color = target1;
                break;

            case 1:
                cubeTwo.material.color = target2;
                break;

            case 2:
                cubeThree.material.color = target3;
                break;
        }
    }


    //private void HandleTriggerPress()
    //{
    //    if (triggerPressCount == 1)
    //    {
    //        cubeOne.material.color = target3;
    //    }
    //    else if (triggerPressCount == 2)
    //    {
    //        cubeTwo.material.color = target2;
    //    }
    //    else if (triggerPressCount == 3)
    //    {
    //        cubeThree.material.color = target1;
    //    }

//    else
//    {
//        cubeOne.material.color = defaultColor;
//        cubeTwo.material.color = defaultColor;
//        cubeThree.material.color = defaultColor;
//    }
//}

//    private void HandleTriggerPress()
//    {
//        triggerPressCount++;

//        int index = triggerPressCount % 3;

//        switch (index)
//        {
//            case 1:
//                cubeOne.material.color = target1;
//                break;

//            case 2:
//                cubeTwo.material.color = target2;
//                break;

//            case 0:
//                cubeThree.material.color = target3;
//                break;
//        }
//    }
}



