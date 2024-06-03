using System.Collections.Generic;
using System.Linq;
using Redbean.Bundle;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Redbean.Singleton
{
	public class AddressableSingleton : ISingleton
	{
		private readonly Dictionary<string, AddressableAsset> assetsGroup = new();

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
			var releaseList = new List<string>();
			foreach (var asset in assetsGroup)
			{
				var removeList = (from reference in asset.Value.References where !reference.Value select reference.Key).ToArray();
				foreach (var remove in removeList)
					asset.Value.References.Remove(remove);
				
				if (asset.Value.References.Any())
					return;

				releaseList.Add(asset.Key);
			}

			foreach (var release in releaseList)
			{
				if (assetsGroup.Remove(release, out var removeBundle))
					removeBundle.Release();
			}
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