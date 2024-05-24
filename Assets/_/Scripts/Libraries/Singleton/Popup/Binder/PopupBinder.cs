using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Redbean.Popup
{
	public class PopupBinder : ISingleton
	{
		private readonly Dictionary<string, PopupBase> popupCollection = new();
		private readonly Canvas canvas;
		private readonly CanvasScaler canvasScaler;
		private readonly GraphicRaycaster raycaster;
		private readonly Transform popupParent;

		public PopupBase CurrentPopup => popupCollection.Values.Last();

		public PopupBinder()
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

		public T Open<T>() where T : PopupBase
		{
			var popup = Instantiate(Resources.Load<GameObject>($"Popup/{typeof(T).Name}")).GetComponent<T>() as PopupBase;
			
			while (true)
			{
				var id = $"{Guid.NewGuid()}";
				if (popupCollection.ContainsKey(id))
					continue;

				popup.Guid = $"{Guid.NewGuid()}";
				break;
			}
			
			popupCollection.Add(popup.Guid, popup);
			return popupCollection[popup.Guid] as T;
		}

		public object Open(Type type)
		{
			var popup = Instantiate(Resources.Load<GameObject>($"Popup/{type.Name}")).GetComponent(type) as PopupBase;
			
			while (true)
			{
				var id = $"{Guid.NewGuid()}";
				if (popupCollection.ContainsKey(id))
					continue;

				popup.Guid = $"{Guid.NewGuid()}";
				break;
			}
			
			popupCollection.Add(popup.Guid, popup);
			return popupCollection[popup.Guid];
		}

		public void CurrentPopupClose()
		{
			var id = CurrentPopup.Guid;
			
			CurrentPopup.Destroy();
			popupCollection.Remove(id);
		}
		
		public void Close(string id)
		{
			popupCollection[id].Destroy();
			popupCollection.Remove(id);
		}

		public void AllClose()
		{
			foreach (var popup in popupCollection.Values)
				popup.Destroy();
			popupCollection.Clear();
		}
		
		public T Get<T>(string id) where T : class => popupCollection[id] as T;
		
		public PopupBase Get(string id) => popupCollection[id];

		private GameObject Instantiate(GameObject gameObject) => GameObject.Instantiate(gameObject, popupParent);
	}
}