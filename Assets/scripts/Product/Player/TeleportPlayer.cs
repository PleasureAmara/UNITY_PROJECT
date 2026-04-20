using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

namespace localizer.product.player
{
    /// <summary>
    /// Manually teleport the player to a specific anchor
    /// </summary>
    public class TeleportPlayer : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Drag the Teleport gameobject under the XR origin")]
        private TeleportationProvider provider;

        /// <summary>
        /// We use this field to track if the teleportation request was successful, and it activates only after the user
        /// has teleported, with this we can control when to render the introduction audios and screens. 
        /// </summary>
        public bool hasTeleported;

        public void OnEnable()
        {
            provider.locomotionStarted += ManageLocomotionStart;
            provider.locomotionEnded += ManageLocomotionEnd;
        }

        public void OnDisable()
        {
            provider.locomotionStarted -= ManageLocomotionStart;
            provider.locomotionEnded -= ManageLocomotionEnd;
        }

        public void RequestToTeleportToAnchor(TeleportationAnchor anchor)
        {
            Transform anchorTransform = anchor.teleportAnchorTransform;

            var request = new TeleportRequest
            {
                requestTime = Time.time,
                matchOrientation = anchor.matchOrientation,
                destinationPosition = anchorTransform.position,
                destinationRotation = anchorTransform.rotation
            };

            provider.QueueTeleportRequest(request);
        }

        private void ManageLocomotionEnd(LocomotionProvider locomotionProvider)
        {
            Debug.Log("Locomotion has ended.");
            hasTeleported = true;
        }

        private void ManageLocomotionStart(LocomotionProvider locomotionProvider)
        {
            Debug.Log("Locomotion has started.");
            //hasTeleported = false;
        }
    }
}
