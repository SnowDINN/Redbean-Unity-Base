using System;
using System.Collections.Generic;
using Redbean.Core;
using UnityEngine;

namespace Redbean.Popup
{
	public class PopupManager : ISingleton
	{
		private readonly Dictionary<Type, PopupBase> popupCollection = new();
		private Transform PopupParent;

		public PopupManager()
		{
			var go = new GameObject("[Popup]");
			PopupParent = go.transform;
		}

		public T Open<T>() => Instantiate(Resources.Load<GameObject>($"Popup/{typeof(T).Name}")).GetComponent<T>();
		
		public object Open(Type type) => Instantiate(Resources.Load<GameObject>($"Popup/{type.Name}")).GetComponent(type);
		
		public T Get<T>() where T : class => popupCollection[typeof(T)] as T;
		
		public PopupBase Get(Type type) => popupCollection[type];

		public void Close<T>()
		{
			popupCollection[typeof(T)].Destroy();
			popupCollection.Remove(typeof(T));
		}
		
		public void Close(Type type)
		{
			popupCollection[type].Destroy();
			popupCollection.Remove(type);
		}
		
		public void Dispose() { }

		private GameObject Instantiate(GameObject gameObject) => GameObject.Instantiate(gameObject);
	}
}