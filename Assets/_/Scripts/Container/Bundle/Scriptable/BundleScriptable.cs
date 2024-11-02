using System.Collections.Generic;
using Redbean.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Redbean
{
	[CreateAssetMenu(fileName = "BundleScriptable", menuName = "Redbean/Library/BundleScriptable")]
	public class BundleScriptable : ScriptableBase
	{
		[Header("Get addressable information during runtime")]
		public string[] Labels;
	}
	
	public class BundleAsset
	{
		public Dictionary<int, Object> References = new();
		public Object Asset = new();

		public void Release()
		{
			foreach (var reference in References.Values)
				Object.Destroy(reference);
			
			Addressables.Release(Asset);
		}
	}
}