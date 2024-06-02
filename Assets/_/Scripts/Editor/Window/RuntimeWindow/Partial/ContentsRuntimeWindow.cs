using System;
using Redbean.Container;
using Redbean.Popup.Content;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Redbean.Editor
{
	internal partial class RuntimeWindow
	{
		private const string ExampleTitle = "Example";
		
		[TabGroup(ContentsTab), Title(ExampleTitle), DisableInEditorMode, Button("Exception")]
		private void ThrowException()
		{
			throw new Exception("An exception has occurred.");
		}

		[TabGroup(ContentsTab), Title(ExampleTitle), DisableInEditorMode, Button("Load Bundle")]
		private async void LoadBundle()
		{
			var go = new GameObject("[Bundle]");
			var canvas = go.AddComponent<Canvas>();
			var canvasScaler = go.AddComponent<CanvasScaler>();
			var raycaster = go.AddComponent<GraphicRaycaster>();

			canvas.renderMode = RenderMode.ScreenSpaceCamera;
			canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
			canvasScaler.referenceResolution = new Vector2(720, 1440);

			var popup = await AddressableContainer.GetPopup<PopupException>();
			Instantiate(popup, go.transform);
		}
	}
}