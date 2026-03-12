using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

namespace localizer.product.player
{
    /// <summary>
    /// Manually teleport the player to a specific anchor
    /// </summary>
    public class TeleportPlayer : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="anchor">The provider used to request the teleportation</param>
        /// <param name="provider">The anchor the player is teleported to</param>
        public void Teleport(TeleportationAnchor anchor, TeleportationProvider provider)
        {
            Transform anchorTransform = anchor.teleportAnchorTransform;

            TeleportRequest request = new()
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
