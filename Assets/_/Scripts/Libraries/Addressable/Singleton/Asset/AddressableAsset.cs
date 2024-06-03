using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Redbean.Bundle
{
	public class AddressableAsset
	{
		public Dictionary<int, Object> References = new();
		public Object Asset;

		public void Release()
		{
			foreach (var reference in References.Values)
				Object.Destroy(reference);
			
			Addressables.Release(Asset);
		}
	}
}