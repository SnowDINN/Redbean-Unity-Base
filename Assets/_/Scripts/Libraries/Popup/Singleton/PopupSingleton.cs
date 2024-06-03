using System;
using System.Collections.Generic;
using System.Linq;
using Redbean.Popup;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Redbean.Singleton
{
	public class PopupSingleton : ISingleton
	{
		private readonly Dictionary<string, PopupBase> popupCollection = new();
		private readonly Canvas canvas;
		private readonly CanvasScaler canvasScaler;
		private readonly GraphicRaycaster raycaster;
		private readonly Transform popupParent;

		public PopupBase CurrentPopup => popupCollection.Values.Last();

		public PopupSingleton()
		{
			var go = new GameObject("[Popup]");
			canvas = go.AddComponent<Canvas>();
			canvasScaler = go.AddComponent<CanvasScaler>();
			raycaster = go.AddComponent<GraphicRaycaster>();

			canvas.renderMode = RenderMode.ScreenSpaceCamera;
			canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
			canvasScaler.referenceResolution = new Vector2(720, 1440);
			
			Object.DontDestroyOnLoad(go);
			
			popupParent = go.transform;

			SceneManager.activeSceneChanged += OnActiveSceneChanged;
		}
		
		public void Dispose()
		{
			SceneManager.activeSceneChanged -= OnActiveSceneChanged;
		}

		private void OnActiveSceneChanged(Scene before, Scene after)
		{
			canvas.worldCamera = Camera.main;
			canvas.sortingLayerName = "Popup";
		}

		public object Open(Type type)
		{
			var bundle = AddressableWrapper.LoadGameObjectAsync(AddressableWrapper.GetPopupPath(type));
			var popup = Instantiate(bundle.Value).GetComponent(type) as PopupBase;
			
			while (true)
			{
				var id = $"{Guid.NewGuid()}";
				if (popupCollection.ContainsKey(id))
					continue;
				
				popup.Guid = $"{Guid.NewGuid()}";
				popup.Bundle = bundle;
				break;
			}
			
			popupCollection.Add(popup.Guid, popup);
			return popupCollection[popup.Guid];
		}
		
		public T Open<T>() where T : PopupBase
		{
			return Open(typeof(T)) as T;
		}
		
		public void Close(string id)
		{
			popupCollection.Remove(id, out var popup);
			popup.Destroy();
		}

		public void AllClose()
		{
			foreach (var popup in popupCollection.Values)
				Close(popup.Guid);
		}
		
		public void CurrentPopupClose()
		{
			Close(CurrentPopup.Guid);
		}
		
		public T Get<T>(string id) where T : class => popupCollection[id] as T;
		
		public PopupBase Get(string id) => popupCollection[id];

		private GameObject Instantiate(GameObject gameObject) => GameObject.Instantiate(gameObject, popupParent);
	}
}