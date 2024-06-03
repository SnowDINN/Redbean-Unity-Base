using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Redbean.Container
{
	public class AddressableSingleton : ISingleton
	{
		private readonly Dictionary<string, Object> Bundles = new();
		
		public async Task<GameObject> GetPopup<T>() =>
			await LoadBundle($"Popup/{typeof(T).Name}.prefab");
		
		public async Task<GameObject> GetPopup(Type type) =>
			await LoadBundle($"Popup/{type.Name}.prefab");
		
		public void ReleasePopup<T>() =>
			ReleaseBundle($"Popup/{typeof(T).Name}.prefab");
		
		public void ReleasePopup(Type type) =>
			ReleaseBundle($"Popup/{type.Name}.prefab");

		public void Dispose() => Bundles.Clear();

		private async Task<GameObject> LoadBundle(string key)
		{
			var go = await Addressables.LoadAssetAsync<GameObject>(key).Task;
			
#if UNITY_EDITOR
			var uguis = go.GetComponentsInChildren<TextMeshProUGUI>(true);
			foreach (var ugui in uguis)
			{
				ReplaceShader(ugui.material);
				ReplaceShader(ugui.materialForRendering);

				if (ugui.spriteAsset)
					ReplaceShader(ugui.spriteAsset.material);
			}
#endif

			Bundles.Add(key, go);
			return go;
		}

		private void ReleaseBundle(string key)
		{
			Bundles.Remove(key, out var go);
			Addressables.Release(go);
		}
		
		private static void ReplaceShader(Material material)
		{
			if (!material)
				return;
			
			var shader = Shader.Find(material.shader.name);
			if (shader)
				material.shader = shader;
		}
	}
}