using EssentialPackages.LightingEditor.Editor.Classes;
using UnityEngine;

namespace EssentialPackages.LightingEditor.Editor
{
	[CreateAssetMenu(fileName = "SceneLightingSpecification", menuName = "Essential/LightingEditor/SceneLightingSpecification", order = 1)]
	public class SceneLightingSpecification : ScriptableObject
	{
		
#if UNITY_2018_2_4
		
		[Header("Environment")]
		[SerializeField] private Environment _environment;
		
		[Header("Realtime Lighting")]
		[SerializeField] private RealtimeLighting _realtimeLighting;

		[Header("Mixed Lighting")]
		[SerializeField] private MixedLighting _mixedLighting;

		[Header("Lightmapping Settings")]
		[SerializeField] private LightmappingSettings _lightmappingSettings;
		
		[Header("Other Settings")]
		[SerializeField] private OtherSettings _otherSettings;
		
		[Header("Debug Settings")]
		[SerializeField] private DebugSettings _debugSettings;

		public Environment Environment => _environment;

#endif
		
	}
}


