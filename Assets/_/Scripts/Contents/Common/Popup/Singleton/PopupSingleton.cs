using System;
using System.Collections.Generic;
using System.Linq;
using Redbean.Bundle;
using Redbean.Popup;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Redbean.Singleton
{
	public class PopupSingletonContainer : ISingletonContainer
	{
		private BundleSingletonContainer Bundle => SingletonContainer.GetSingleton<BundleSingletonContainer>();
		
		private readonly Dictionary<int, PopupBase> popupsGroup = new();
		private readonly Transform popupParent;
		private readonly Canvas canvas;

		public PopupBase CurrentPopup => popupsGroup.Values.Last();

		public PopupSingletonContainer()
		{
			var go = new GameObject("[Popup]");
			Object.DontDestroyOnLoad(go);
			
			canvas = go.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceCamera;
			
			var canvasScaler = go.AddComponent<CanvasScaler>();
			canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
			canvasScaler.referenceResolution = new Vector2(720, 1440);
			
			go.AddComponent<GraphicRaycaster>();
			
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
			var go = Bundle.LoadAsset<GameObject>(AddressableSettings.GetPopupPath(type), popupParent);
			var popup = go.GetComponent(type) as PopupBase;
			popup.Guid = go.GetInstanceID();
			
			popupsGroup.Add(popup.Guid, popup);
			return popupsGroup[popup.Guid];
		}
		
		public void Close(int id)
		{
			if (!popupsGroup.Remove(id, out var popup))
				return;
			
			Bundle.Release(AddressableSettings.GetPopupPath(popup.GetType()), popup.Guid);
		}

		public void AllClose()
		{
			foreach (var popup in popupsGroup.Values)
				Close(popup.Guid);
		}
		
		public T Open<T>() where T : PopupBase => Open(typeof(T)) as T;
		
		public T GetPopup<T>(int id) where T : class => popupsGroup[id] as T;
		
		public PopupBase GetPopup(int id) => popupsGroup[id];
		
		public void CurrentPopupClose() => Close(CurrentPopup.Guid);
	}
}