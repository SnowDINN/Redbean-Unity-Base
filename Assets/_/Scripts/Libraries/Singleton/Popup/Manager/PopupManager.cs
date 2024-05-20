﻿using System;
using System.Collections.Generic;
using Redbean.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Redbean.Popup
{
	public class PopupManager : ISingleton
	{
		private readonly Dictionary<Type, PopupBase> popupCollection = new();
		private readonly Canvas canvas;
		private readonly CanvasScaler canvasScaler;
		private readonly GraphicRaycaster raycaster;
		private readonly Transform popupParent;

		public PopupManager()
		{
			var go = new GameObject("[Popup Canvas]");
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
			popupCollection.Add(typeof(T), Instantiate(Resources.Load<GameObject>($"Popup/{typeof(T).Name}")).GetComponent<T>());
			return popupCollection[typeof(T)] as T;
		}

		public object Open(Type type)
		{
			popupCollection.Add(type, Instantiate(Resources.Load<GameObject>($"Popup/{type.Name}")).GetComponent(type) as PopupBase);
			return popupCollection[type];
		}

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
		
		public T Get<T>() where T : class => popupCollection[typeof(T)] as T;
		
		public PopupBase Get(Type type) => popupCollection[type];

		private GameObject Instantiate(GameObject gameObject) => GameObject.Instantiate(gameObject, popupParent);
	}
}