using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Redbean.Container;
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

		public async Task<object> Open(Type type)
		{
			var go = await AddressableContainer.GetPopup(type);
			var popup = Instantiate(go).GetComponent(type) as PopupBase;
			
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
		
		public async Task<T> Open<T>() where T : PopupBase
		{
			return await Open(typeof(T)) as T;
		}
		
		public void Close(string id)
		{
			popupCollection.Remove(id, out var popup);
			popup.Destroy();
			
			AddressableContainer.ReleasePopup(popup.GetType());
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