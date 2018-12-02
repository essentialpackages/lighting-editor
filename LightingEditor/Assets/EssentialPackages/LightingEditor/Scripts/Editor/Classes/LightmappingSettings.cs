using System;
using UnityEngine;

namespace EssentialPackages.LightingEditor.Editor.Classes
{
    [Serializable]
    public class LightmappingSettings
    {
		
#if UNITY_2018_2_4

        [SerializeField] private LightMapper _lightmapper;
        [Space(10)]
        [SerializeField] private int _indirectResolution = 2;
        [SerializeField] private float _lightmapResolution = 10;
        [SerializeField] private int _lightmapPadding = 2;
        [SerializeField] private string _lightmapSize = "512";
        [SerializeField] private bool _compressLightmaps = true;
        [SerializeField] private bool _ambientOcclusion = false;
        [SerializeField] private string _directionalMode = "Directional";
        [Range(0.0f, 5.0f)]
        [SerializeField] private float _indirectIntensity = 1.0f;
        [Range(1.0f, 10.0f)]
        [SerializeField] private float _albedoBoost = 1.0f;
        [SerializeField] private string _lightmapParameters = "Default-Medium";

#endif

    }
}
