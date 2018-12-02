using System;
using UnityEngine;

namespace EssentialPackages.LightingEditor.Editor.Classes
{
	[Serializable]
	public class EnvironmentLighting
	{
		
#if UNITY_2018_2_4
		
		[Tooltip("[Skybox, Gradient, Color]")]
		[SerializeField] private string _source = "Skybox";
		
		[Tooltip("Source must be set to Skybox")]
		[Range(0.0f, 8.0f)]
		[SerializeField] private float _intensityMultiplier = 1.0f;
		
		[Tooltip("Source must be set to Gradient")]
		[ColorUsageAttribute(false, true)]
		[SerializeField] private Color32 _skyColor = new Color32(54, 58, 66, 255);
		[Tooltip("Source must be set to Gradient")]
		[ColorUsageAttribute(false, true)]
		[SerializeField] private Color32 _equatorColor = new Color32(29, 32, 34, 255);
		[Tooltip("Source must be set to Gradient")]
		[ColorUsageAttribute(false, true)]
		[SerializeField] private Color32 _groundColor = new Color32(12, 11, 9, 255);
		
		[Tooltip("Source must be set to Color")]
		[ColorUsageAttribute(false, true)]
		[SerializeField] private Color32 _ambientColor = new Color32(54, 58, 66, 255);
		
		[Tooltip("[Realtime, Baked] + only active when Realtime Global Illumination and Baked Global Illumnination is active")]
		[SerializeField] private string _ambientMode = "Baked";
		
#endif

	}
}
