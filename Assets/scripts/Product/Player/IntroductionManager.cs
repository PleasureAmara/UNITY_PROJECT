using localizer.product.player;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

namespace localizer.product.player
{
    class AnchorModel
    {
        public string AnchorName { get; set; }
        public TeleportationAnchor teleportationAnchor { get; set; }
    }
    public class IntroductionManager : MonoBehaviour
    {
    
        private Vector3 playerStartingPosition = new(1379, 52, -329);
        private TeleportPlayer teleportPlayer;
        private SoundController soundController;

        [SerializeField] private TeleportationProvider teleportationProvider;
        [SerializeField] private TeleportationAnchor introAnchor;
        [SerializeField] private AudioSource introAudio;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
        {
            teleportPlayer = new();
            soundController = new();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void RunIntroOne()
        {
            teleportPlayer.Teleport(introAnchor, teleportationProvider);

        }
    }

}

