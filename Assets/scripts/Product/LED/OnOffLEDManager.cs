using UnityEngine;
using System;
using System.Threading;
using System.Collections;

namespace localizer.product.led
{
    public class OnOffLEDManager : MonoBehaviour
    {
        [SerializeField] private string colorMaterialName;
        private Material[] allMaterial;
        private Material targetMaterial;

        /// <summary>
        /// this field is used by other scripts to activate the emissive property of the LED.
        /// </summary>
        public bool isEmissionEnabled;

        /// <summary>
        /// it is set by other scripts to have more control over the emissive feature of the Light.
        /// </summary>
        public bool isBlinking;
        private void Start()
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            allMaterial =meshRenderer.materials;
            if (allMaterial == null) return;

            foreach (Material colorMaterial in allMaterial)
            {
                if (colorMaterial.name.Contains(colorMaterialName))
                {
                    targetMaterial = colorMaterial;
                    break;
                }
            }

            if (targetMaterial != null) StartCoroutine(MakeEmissive());
     
        }

        IEnumerator MakeEmissive()
        {
            while (isEmissionEnabled)
            {
                targetMaterial.EnableKeyword("_EMISSION");
                yield return new WaitForSeconds(1);

                if (isBlinking)
                {
                    targetMaterial.DisableKeyword("_EMISSION");
                    yield return new WaitForSeconds(1);
                }
            }
        
        }

    }
}
