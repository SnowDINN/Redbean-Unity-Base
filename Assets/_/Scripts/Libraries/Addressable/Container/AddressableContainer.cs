using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Redbean.Container
{
	public class AddressableContainer
	{
		public static async Task<GameObject> GetPopup<T>() =>
			await LoadBundle($"Popup/{typeof(T).Name}.prefab");
		
		public static async Task<GameObject> GetPopup(Type type) =>
			await LoadBundle($"Popup/{type.Name}.prefab");

		private static async Task<GameObject> LoadBundle(string key)
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

			return go;
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