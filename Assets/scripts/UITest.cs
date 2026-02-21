using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(TestClick);
    }

    void TestClick()
    {
        Debug.Log("NEXT BUTTON CLICKED");
    }
}
