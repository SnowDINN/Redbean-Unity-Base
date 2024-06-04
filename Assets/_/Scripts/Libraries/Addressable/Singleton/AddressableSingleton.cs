using System.Collections.Generic;
using System.Linq;
using Redbean.Bundle;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Redbean.Singleton
{
	public class AddressableSingleton : ISingleton
	{
		private Dictionary<string, AddressableAsset> assetsGroup = new();

		public T LoadAsset<T>(string key, Transform parent = null) where T : Object
		{
			var bundle = new AddressableAsset();
			
			if (assetsGroup.TryGetValue(key, out var assetBundle))
				bundle = assetBundle;
			else
			{
				bundle = LoadBundle<T>(key);
				assetsGroup[key] = bundle;
			}


			var asset = Object.Instantiate(bundle.Asset as T, parent);
			assetsGroup[key].References[asset.GetInstanceID()] = asset;
			
			return asset;
		}

		public void Release(string key, int instanceId)
		{
#region Try Get Asset

			if (!assetsGroup.TryGetValue(key, out var assetBundle))
				return;

			if (assetBundle.References.Remove(instanceId, out var go))
				Object.Destroy(go);

#endregion

#region Check Use Reference

			if (assetBundle.References.Any())
				return;

			if (assetsGroup.Remove(key, out var removeBundle))
				removeBundle.Release();

#endregion
		}

		public void AutoRelease()
		{
			var assetsArray = assetsGroup.ToList();
			for (var i = 0; i < assetsArray.Count; i++)
			{
				var referenceArray = assetsArray[i].Value.References.ToList();
				for (var j = 0; j < referenceArray.Count; j++)
				{
					if (!referenceArray[j].Value)
						referenceArray.RemoveAt(j);
				}
				
				assetsArray[i].Value.References = referenceArray.ToDictionary(_ => _.Key, _ => _.Value);
				if (!assetsArray[i].Value.References.Any())
				{
					assetsArray[i].Value.Release();
					assetsArray.RemoveAt(i);
				}
			}
			
			assetsGroup = assetsArray.ToDictionary(_ => _.Key, _ => _.Value);
		}
		

		public void Dispose()
		{
			assetsGroup.Clear();
		}

		private AddressableAsset LoadBundle<T>(string key) where T : Object
		{
			var value = Addressables.LoadAssetAsync<T>(key).WaitForCompletion();
			var bundle = new AddressableAsset
			{
				Asset = value,
			};

			return bundle;
		}
	}
}