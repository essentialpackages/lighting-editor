using System;
using UnityEngine;

namespace EssentialPackages.LightingEditor.Editor.Classes
{
    [Serializable]
    public class LightMapper
    {

#if UNITY_2018_2_4

        [Tooltip("[Enlighten, Progressive]")]
        [SerializeField] private string _lightMapper = "Progressive";
        [SerializeField] private bool _prioritizeView = true;
        [SerializeField] private int _directSamples = 32;
        [SerializeField] private int _indirectSamples = 256;
        [Tooltip("[None, 1, 2, 3, 4]")]
        [SerializeField] private string _bounces = "2";
        [Tooltip("[None, Auto, Advanced]")]
        [SerializeField] private string _filtering = "Auto";

        [Tooltip("[Gaussian, A-Torus, None] + only active when Filtering is set to Advanced")]
        [SerializeField] private string _directFilter = "Gaussian";

        [Tooltip("Only active when Filtering is set to Advanced and Direct Filter is set to Gaussian")]
        [Range(0, 5)]
        [SerializeField] private int _directRadius = 1;

        [Tooltip("Only active when Filtering is set to Advanced and Direct Filter is set to A-Torus")]
        [Range(0.0f, 2.0f)]
        [SerializeField] private float _directSigma = 0.5f;

        [Tooltip("[Gaussian, A-Torus, None] + only active when Filtering is set to Advanced")]
        [SerializeField] private string _indirectFilter = "Gaussian";

        [Tooltip("Only active when Filtering is set to Advanced and Indirect Filter is set to Gaussian")]
        [Range(0, 5)]
        [SerializeField] private int _indirectRadius = 5;
        
        [Tooltip("Only active when Filtering is set to Advanced and Indirect Filter is set to A-Torus")]
        [Range(0.0f, 2.0f)]
        [SerializeField] private float _indirectSigma = 2.0f;
        
        [Tooltip("[Gaussian, A-Torus, None] + Only active when Filtering is set to Advanced and Ambient Occlusion is set to active")]
        [SerializeField] private string _ambientOcclusionFilter = "Gaussian";

        [Tooltip("Only active when Filtering is set to Advanced, Ambient Occlusion is set to active and Ambient Occlusion Filter is set to Gaussian")]
        [Range(0, 5)]
        [SerializeField] private int _ambientOcclusionRadius = 2;

        [Tooltip("Only active when Filtering is set to Advanced, Ambient Occlusion is set to active and Ambient Occlusion Filter is set to A-Torus")]
        [Range(0.0f, 2.0f)]
        [SerializeField] private float _ambientOcclusionSigma = 1.0f;

#endif

    }
}
