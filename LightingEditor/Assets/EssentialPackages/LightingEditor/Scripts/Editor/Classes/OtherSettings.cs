using System;
using UnityEngine;

namespace EssentialPackages.LightingEditor.Editor.Classes
{
	[Serializable]
	public class OtherSettings
	{
		
#if UNITY_2018_2_4

		[SerializeField] private bool _fog = false;

		[Tooltip("Only active when Fog is active")]
		[SerializeField] private Color _color = new Color32(128, 128, 128, 255);
		[Tooltip("[Linear, Exponential, Exponential Squared] + only active when Fog is active")]
		[SerializeField] private string _mode = "Exponential Squared";
		[Tooltip("Only active when Fog is active and _mode is set to Linear")]
		[SerializeField] private float _start;
		[Tooltip("Only active when Fog is active and _mode is set to Linear")]
		[SerializeField] private float _end = 300f;
		[Tooltip("Only active when Fog is active and _mode is set to Exponential or Exponential Squared")]
		[SerializeField] private float _density = 0.01f;
		
		[SerializeField] private Texture2D _haloTexture;
		[Range(0.0f, 1.0f)]
		[SerializeField] private float _haloStrength = 0.5f;
		[SerializeField] private float _flareFadeSpeed = 3.0f;
		[Range(0.0f, 1.0f)]
		[SerializeField] private float _flareFadeStrength = 1.0f;
		[Tooltip("When cookie is null, Unity will automatically use 'Soft' as default cookie")]
		//[SerializeField] private string _spotCookie = "None";
		[SerializeField] private Texture2D _spotCookie;

#endif

	}
}
