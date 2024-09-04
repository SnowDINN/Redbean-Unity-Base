using System;
using System.Collections.Generic;
using System.Linq;
using Redbean.Bundle;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Redbean.Popup
{
	public class PopupManager : ISingleton
	{
		private readonly Dictionary<int, PopupBase> popups = new();
		private readonly Transform popupParent;
		private readonly Canvas canvas;

		public PopupBase CurrentPopup => popups.Values.Last();

		public PopupManager()
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
			SetPopupCanvasParameter();

			SceneManager.activeSceneChanged += OnActiveSceneChanged;
			SceneManager.sceneLoaded += OnSceneLoaded;
		}
		
		public void Dispose()
		{
			SceneManager.activeSceneChanged -= OnActiveSceneChanged;
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}

		private void OnActiveSceneChanged(Scene before, Scene after) => SetPopupCanvasParameter();

		private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) => SetPopupCanvasParameter();

		private void SetPopupCanvasParameter()
		{
			canvas.worldCamera = Camera.main;
			canvas.sortingLayerName = "Popup";
		}
		
		public object AssetOpen(Type type)
		{
			var resource = Resources.Load<GameObject>($"Popup/{type.Name}");
			var go = GameObject.Instantiate(resource, popupParent);
			var popup = go.GetComponent(type) as PopupBase;
			popup.Type = PopupType.Asset;
			popup.Guid = go.GetInstanceID();
			
			popups.Add(popup.Guid, popup);
			return popups[popup.Guid];
		}

		public object BundleOpen(Type type)
		{
			var go = BundleContainer.LoadAsset<GameObject>(AddressableSettings.GetPopupPath(type), popupParent);
			var popup = go.GetComponent(type) as PopupBase;
			popup.Type = PopupType.Bundle;
			popup.Guid = go.GetInstanceID();
			
			popups.Add(popup.Guid, popup);
			return popups[popup.Guid];
		}
		
		public void Close(int id)
		{
			if (!popups.Remove(id, out var popup))
				return;

			switch (popup.Type)
			{
				case PopupType.Asset:
					Object.Destroy(popup.gameObject);
					break;
				
				case PopupType.Bundle:
					BundleContainer.Release(AddressableSettings.GetPopupPath(popup.GetType()), popup.Guid);
					break;
			}
		}

		public void AllClose()
		{
			foreach (var popup in popups.Values)
				Close(popup.Guid);
		}
		
		public T AssetOpen<T>() where T : PopupBase => AssetOpen(typeof(T)) as T;
		public T BundleOpen<T>() where T : PopupBase => BundleOpen(typeof(T)) as T;
		
		public T GetPopup<T>(int id) where T : class => popups[id] as T;
		
		public PopupBase GetPopup(int id) => popups[id];
		
		public void CurrentPopupClose() => Close(CurrentPopup.Guid);
	}
}