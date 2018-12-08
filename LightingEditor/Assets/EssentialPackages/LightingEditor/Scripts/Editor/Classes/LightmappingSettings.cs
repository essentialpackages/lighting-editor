using System;
using UnityEngine;

namespace EssentialPackages.LightingEditor.Editor.Classes
{
    // Only active when Baked Global Illumination is active
    [Serializable]
    public class LightmappingSettings
    {
		
#if UNITY_2018_2_4

        [Tooltip("Only active when Baked Global Illumination is active")]
        [SerializeField] private LightMapper _lightmapper;
        [Space(10)]
        [Tooltip("Only active when Realtime Global Illumnination is active")] // TODO 
        [SerializeField] private float _indirectResolution = 2;
        [Tooltip("Only active when Baked Global Illumnination is active")]
        [SerializeField] private float _lightmapResolution = 10;
        [Tooltip("Only active when Baked Global Illumnination is active")]
        [SerializeField] private int _lightmapPadding = 2;
        [Tooltip("[32, 64, 128, 256, 512, 1024, 2048, 4096] + only active when Baked Global Illumnination is active")]
        [SerializeField] private string _lightmapSize = "512";
        [Tooltip("Only active when Baked Global Illumnination is active")]
        [SerializeField] private bool _compressLightmaps = true;
        [Tooltip("Only active when Baked Global Illumnination is active")]
        [SerializeField] private bool _ambientOcclusion = false;
        [Tooltip("Only active when Baked Global Illumnination is active")]
        [SerializeField] private bool _finalGather = false;
        
        [Tooltip("Only active when Ambient Occlusion is set to active")]
        [SerializeField] private float _maxDistance = 1.0f;
        [Tooltip("Only active when Ambient Occlusion is set to active")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _indirectContribution = 1.0f;
        [Tooltip("Only active when Ambient Occlusion is set to active")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _directContribution = 0.0f;

        [Tooltip("Only active when Final Gather is set to active")]
        [SerializeField] private int _rayCount = 256;
        [Tooltip("Only active when Final Gather is set to active")]
        [SerializeField] private bool _denoising = true;
        
        [Tooltip("[Non-Directional, Directional] + only active when Realtime Global Illumination or Baked Global Illumination is active")]
        [SerializeField] private string _directionalMode = "Directional";
        [Tooltip("Only active when Realtime Global Illumination or Baked Global Illumination is active")]
        [Range(0.0f, 5.0f)]
        [SerializeField] private float _indirectIntensity = 1.0f;
        [Tooltip("Only active when Realtime Global Illumination or Baked Global Illumination is active")]
        [Range(1.0f, 10.0f)]
        [SerializeField] private float _albedoBoost = 1.0f;
        [Tooltip("[Default-Medium, Default-HighResolution, Default-LowResolution, Default-VeryLowResolution, <Create New ...>] + only active when Realtime Global Illumination or Baked Global Illumination is active")]
        [SerializeField] private string _lightmapParameters = "Default-Medium";

#endif

    }
}
