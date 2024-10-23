using System;
using Redbean.Base;
using UnityEngine;

namespace Redbean.Bundle
{
	[CreateAssetMenu(fileName = "BundleScriptable", menuName = "Redbean/Library/BundleScriptable")]
	public class BundleScriptable : ScriptableObject
	{
		[Header("Get addressable information during runtime")]
		public string[] Labels;
	}

	public class BundleReferencer : ScriptableBase<BundleScriptable>
	{
		public static string GetPopupPath(Type type) => $"Popup/{type.Name}.prefab";
		
		public static string[] Labels
		{
			get => Scriptable.Labels;
			set
			{
				Scriptable.Labels = value;
				
#if UNITY_EDITOR
				Save();
#endif
			}
		}
	}
}