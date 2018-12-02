using System;
using UnityEngine;

namespace EssentialPackages.LightingEditor.Editor.Classes
{
	[Serializable]
	public class DebugSettings
	{
		
#if UNITY_2018_2_4

		[SerializeField] private LightProbeVisualization _lightProbeVisualization;
		[Space(10)]
		[SerializeField] private bool _autoGenerate = true;

#endif

	}
}
