using System;
using UnityEngine;

namespace EssentialPackages.LightingEditor.Editor.Classes
{
	[Serializable]
	public class Environment
	{
		
#if UNITY_2018_2_4
		
		[SerializeField] private Material _skyboxMaterial;
		[SerializeField] private Light _sunSource;
		[Space(10)]
		[SerializeField] private EnvironmentLighting _environmentLighting;
		[Space(10)]
		[SerializeField] private EnvironmentReflections _environmentReflections;

#endif

	}
}
