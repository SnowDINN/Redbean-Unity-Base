using System;
using Redbean.Bundle;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Redbean.Singleton
{
	public class AddressableWrapper
	{
		public static string GetPopupPath(Type type) => $"Popup/{type.Name}.prefab";

		public static GameObjectBundle LoadGameObjectAsync(string key)
		{
			var go = Addressables.LoadAssetAsync<GameObject>(key).WaitForCompletion();
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