using System;
using UnityEngine;

namespace EssentialPackages.LightingEditor.Editor.Classes
{
	[Serializable]
	public class RealtimeLighting
	{
				
#if UNITY_2018_2_4

		[SerializeField] private bool _realtimeGlobalIllumination = false;

#endif

	}
}