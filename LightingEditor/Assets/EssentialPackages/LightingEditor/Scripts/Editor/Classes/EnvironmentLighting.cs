using System;
using UnityEngine;

namespace EssentialPackages.LightingEditor.Editor.Classes
{
	[Serializable]
	public class EnvironmentLighting
	{
		
#if UNITY_2018_2_4
		
		[SerializeField] private string _source = "Skybox";
		[Range(0.0f, 8.0f)]
		[SerializeField] private float _intensityMultiplier = 1.0f;
		[SerializeField] private string _ambientMode = "Baked";
		
#endif

	}
}
