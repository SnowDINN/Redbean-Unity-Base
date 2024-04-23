using System;
using Redbean.Rx;
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
		
		public static T GetModel<T>(this MonoBehaviour mono) => 
			Model.Get<T>();
		
		public static T GetSingleton<T>(this MonoBehaviour mono) => 
			Singleton.Get<T>();
		
		public static int GetLocalInt(this MonoBehaviour mono, string key) =>
			GetSingleton<RxDataBinder>().DataGroup.TryGetValue(key, out var value) ? Convert.ToInt32(value) : default;
		
		public static float GetLocalFloat(this MonoBehaviour mono, string key) =>
			GetSingleton<RxDataBinder>().DataGroup.TryGetValue(key, out var value) ? Convert.ToSingle(value) : default;
		
		public static string GetLocalString(this MonoBehaviour mono, string key) =>
			GetSingleton<RxDataBinder>().DataGroup.TryGetValue(key, out var value) ? Convert.ToString(value) : default;

		public static T GetLocalModel<T>(this MonoBehaviour mono, string key) =>
			GetSingleton<RxDataBinder>().DataGroup.TryGetValue(key, out var value) ? (T)value : default;
	}
}