using System;
using UnityEngine;

namespace EssentialPackages.LightingEditor.Editor.Classes
{
    [Serializable]
    public class LightMapper
    {

#if UNITY_2018_2_4

        [SerializeField] private string _lightMapper = "Progressive";
        [SerializeField] private bool _prioritizeView = true;
        [SerializeField] private int _directSamples = 32;
        [SerializeField] private int _indirectSamples = 256;
        [SerializeField] private string _bounces = "2";
        [SerializeField] private string _filtering = "Auto";

#endif

    }
}
