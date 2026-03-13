using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

namespace localizer.product.player
{
    /// <summary>
    /// Manually teleport the player to a specific anchor
    /// </summary>
    public class TeleportPlayer : MonoBehaviour
    {
        // Add a method to accept an anchor parameter
        public void TeleportToAnchor(TeleportationAnchor anchor, TeleportationProvider provider)
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
    }
}
