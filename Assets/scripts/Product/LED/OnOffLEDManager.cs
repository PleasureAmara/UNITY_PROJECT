using UnityEngine;
using System;
using System.Threading;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


//TODO: MAKE ALL LEDs LIGHT UP AND BLINK    



namespace localizer.product.led
{
    public class OnOffLEDManager : MonoBehaviour
    {
        [Tooltip("Attach a string which matches the emissive material attached to the gameobject.")]
        [SerializeField] private string colorMaterialName;

        [Tooltip("Tick box of you want the gameobject to start glowing from the moment the game starts.")]
        [SerializeField] private bool shouldLightFromStart;

        //[Tooltip("Tick if you want the game object to light up the moment its enabled.")]
        /// <summary>
        /// this field is to be used by other scripts to activate the emissive property of the LED.
        /// </summary>
        private bool isEmissive;

        [Tooltip("Tick if you want the gameobject to be blinking the moment it starts emitting light.")]
        /// <summary>
        /// it is to be set by other scripts to have more control over the emissive feature of the Light.
        /// </summary>
        public bool isBlinking;

        [Tooltip("This will only work if the 'is blinking' box is ticked. ")]
        [SerializeField] private float blinkingRate = 1.0f;

        private Material[] allMaterial;
        private Material targetMaterial;
        //private Coroutine makeEmissiveCoroutineHolder;

        //[SerializeField] private bool isLighting;

        private void Start()
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            FindTargetMaterial(meshRenderer);
            
            if (shouldLightFromStart)
            {
                isEmissive = true;
                StartCoroutine(MakeEmissive());
            }
                
     
        }

        public void ManageLighting()
        {
            if (!isEmissive)
            {
                isEmissive = true;
                StartCoroutine(MakeEmissive());
                return;
            }
            isEmissive = false;
        }

        private IEnumerator MakeEmissive()
        {
            while (isEmissive)
            {
                //if (isEmissive)
                //{
                    targetMaterial.EnableKeyword("_EMISSION");
                    yield return new WaitForSeconds(blinkingRate);

                    if (isBlinking)
                    {
                        targetMaterial.DisableKeyword("_EMISSION");
                        yield return new WaitForSeconds(blinkingRate);
                    }
                ////}
                //else
                //{
                //    targetMaterial.DisableKeyword("_EMISSION");
                //    yield return null;
                //}
            }

            targetMaterial.DisableKeyword("_EMISSION");
        }

        private void FindTargetMaterial(MeshRenderer targetRenderer)
        {
            allMaterial = targetRenderer.materials;
            if (allMaterial == null) return;

            foreach (Material colorMaterial in allMaterial)
            {
                if (colorMaterial.name.Contains(colorMaterialName))
                {
                    targetMaterial = colorMaterial;
                    break;
                }
            }
        }

    }
}
