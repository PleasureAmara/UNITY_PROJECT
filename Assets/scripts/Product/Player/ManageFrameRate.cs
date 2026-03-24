using UnityEngine;

namespace localizer.product.player
{
    /// <summary>
    /// this class sets both the application target frames per second (FPS) and the adroid(quest) FPS to a static value. This is to improve on the
    /// neausea felt in the headsets. 
    /// </summary>
    public class ManageFrameRate : MonoBehaviour
    {
        [SerializeField] private int staticframeRate = 70;

        void Start()
        {
            // we set this vertical synchronization count to zero  to make unity repect the manual setting.
            QualitySettings.vSyncCount = 0;

            Application.targetFrameRate = staticframeRate;

//#if UNITY_ANDROID && !UNITY_EDITOR
//OVRPlugin.systemDisplayFrequency = (float)staticframeRate;
//#endif
        }

    }
}
