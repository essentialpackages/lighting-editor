using System;
using UnityEngine;

namespace EssentialPackages.LightingEditor.Editor.Classes
{
    [Serializable]
    public class MixedLighting
    {
		
#if UNITY_2018_2_4

        [SerializeField] private bool _bakedGlobalIllumination = true;
        [Tooltip("[Baked Indirect, Subtractive, Shadowmask] + only active when Baked Global Illumination is active")]
        [SerializeField] private string _lightingMode = "Shadowmask";

        [Tooltip("Only active when Baked Global Illumination is active and Lighting Mode is set to Subtractive")]
        [SerializeField]
        private Color _realtimeShadowColor = new Color(107, 122, 160, 255);

#endif

    }
}
