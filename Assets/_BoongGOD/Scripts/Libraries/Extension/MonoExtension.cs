using Redbean.Base;
using UnityEngine;

namespace Redbean.Extension
{
	public static class MonoExtension
	{
		public static void ActiveGameObject(this Component component, bool value) => 
			component.gameObject.SetActive(value);

		public static void ActiveComponent(this MonoBehaviour mono, bool value) => 
			mono.enabled = value;
		
		public static T GetSingleton<T>(this MonoBehaviour mono) => 
			Singleton.Get<T>();
	}
}