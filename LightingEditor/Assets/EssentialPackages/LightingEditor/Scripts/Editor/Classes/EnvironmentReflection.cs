using System;
using UnityEngine;

namespace EssentialPackages.LightingEditor.Editor.Classes
{
	[Serializable]
	public class EnvironmentReflections
	{
		
#if UNITY_2018_2_4
		
		[SerializeField] private string _source = "Skybox";
		[SerializeField] private string _resolution = "128";
		[SerializeField] private string _compression = "Auto";
		[Range(0.0f, 1.0f)]
		[SerializeField] private float _intensityMultiplier = 1.0f;
		[Range(1, 5)]
		[SerializeField] private int _bounces = 1;
		
#endif
		
	}
}