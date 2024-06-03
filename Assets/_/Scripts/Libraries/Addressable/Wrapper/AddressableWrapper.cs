﻿using System;
using System.Threading.Tasks;
using Redbean.Bundle;
using UnityEngine;
using UnityEngine.AddressableAssets;

#if UNITY_EDITOR
using TMPro;
#endif

namespace Redbean.Singleton
{
	public class AddressableWrapper
	{
		public static string GetPopupPath(Type type) => $"Popup/{type.Name}.prefab";

		public static async Task<GameObjectBundle> LoadGameObjectAsync(string key)
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

			var bundle = new GameObjectBundle
			{
				Value = go,
			};
			
			return bundle;
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