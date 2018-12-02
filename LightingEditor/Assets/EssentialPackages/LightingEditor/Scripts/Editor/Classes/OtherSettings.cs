using System;
using UnityEngine;

namespace EssentialPackages.LightingEditor.Editor.Classes
{
	[Serializable]
	public class OtherSettings
	{
		
#if UNITY_2018_2_4

		[SerializeField] private bool _fog = false;
		[SerializeField] private Texture2D _haloTexture;
		[Range(0.0f, 1.0f)]
		[SerializeField] private float _haloStrength = 0.5f;
		[SerializeField] private float _flareFadeSpeed = 3.0f;
		[Range(0.0f, 1.0f)]
		[SerializeField] private float _flareFadeStrength = 1.0f;
		[Tooltip("When 'None' is selected, Unity will automatically use 'Soft' as default cookie")]
		[SerializeField] private string _spotCookie = "None";

#endif

	}
}
