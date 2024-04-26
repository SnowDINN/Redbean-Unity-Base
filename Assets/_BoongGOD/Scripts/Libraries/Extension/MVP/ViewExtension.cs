using Redbean.Static;
using UnityEngine;

namespace Redbean.Extension
{
	public static partial class Extension
	{
		public static void SetParent(this GameObject go, Transform parent) => 
			go.transform.SetParent(parent);
		
		public static void ActiveGameObject(this Component component, bool value) => 
			component.gameObject.SetActive(value);

		public static void ActiveComponent(this MonoBehaviour mono, bool value) => 
			mono.enabled = value;
		
		/// <summary>
		/// 모델 호출
		/// </summary>
		public static T GetModel<T>(this MonoBehaviour mono) where T : IModel => Model.GetOrAdd<T>();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static T GetSingleton<T>(this MonoBehaviour mono) where T : ISingleton => Singleton.GetOrAdd<T>();
	}
}