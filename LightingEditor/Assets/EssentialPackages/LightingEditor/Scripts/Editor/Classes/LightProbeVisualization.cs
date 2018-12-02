using System;
using UnityEngine;

namespace EssentialPackages.LightingEditor.Editor.Classes
{
	[Serializable]
	public class LightProbeVisualization
	{

#if UNITY_2018_2_4

		[SerializeField] private string _dropdownMenu = "Only Probes Used By Selection";
		[SerializeField] private bool _displayWeights = true;
		[SerializeField] private bool _displayOcclusion = true;

#endif

	}
}
