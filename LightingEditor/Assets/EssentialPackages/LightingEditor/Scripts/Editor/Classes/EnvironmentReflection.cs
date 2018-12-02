using System;
using UnityEngine;

namespace EssentialPackages.LightingEditor.Editor.Classes
{
	[Serializable]
	public class EnvironmentReflections
	{
		
#if UNITY_2018_2_4
		
		[Tooltip("[Skybox, Custom]")]
		[SerializeField] private string _source = "Skybox";
		
		[Tooltip("[16, 32, 64, 128, 256, 512, 1024, 2048] + Source must be set to Skybox")]
		[SerializeField] private string _resolution = "128";

		[Tooltip("Source must be set to Custom")]
		[SerializeField] private Cubemap _cubemap;
		
		[Tooltip("Uncompressed, Compressed, Auto")]
		[SerializeField] private string _compression = "Auto";
		[Range(0.0f, 1.0f)]
		[SerializeField] private float _intensityMultiplier = 1.0f;
		[Range(1, 5)]
		[SerializeField] private int _bounces = 1;
		
#endif
		
	}
}