using System;
using UnityEngine;

namespace EssentialPackages.LightingEditor.Editor.Classes
{
    [Serializable]
    public class MixedLighting
    {
		
#if UNITY_2018_2_4

        [SerializeField] private bool _bakedGlobalIllumination = true;
        [SerializeField] private string _lightingMode = "Shadowmask";

#endif

    }
}
