using System;
using System.Threading;
using Redbean.Base;
using Redbean.Container;
using Redbean.MVP;
using Redbean.Singleton;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif

namespace Redbean
{
	public static partial class Extension
	{
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		private static T GetSingleton<T>() where T : ISingleton => SingletonContainer.GetSingleton<T>();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		private static object GetSingleton(Type type) => SingletonContainer.GetSingleton(type);
		
		/// <summary>
		/// 모델 호출
		/// </summary>
		private static T GetModel<T>() where T : IModel => SingletonContainer.GetSingleton<MvpSingleton>().GetModel<T>();
		
		/// <summary>
		/// 모델 호출
		/// </summary>
		private static object GetModel(Type type) => SingletonContainer.GetSingleton<MvpSingleton>().GetModel(type);
		
#if UNITY_EDITOR
		/// <summary>
		/// 모델 호출
		/// </summary>
		public static T GetModel<T>(this OdinEditorWindow editor) where T : IModel => GetModel<T>();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static T GetSingleton<T>(this OdinEditorWindow editor) where T : ISingleton => GetSingleton<T>();
#endif
		
		/// <summary>
		/// 모델 호출
		/// </summary>
		public static T GetModel<T>(this MonoBase mono) where T : IModel => GetModel<T>();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static T GetSingleton<T>(this MonoBase mono) where T : ISingleton => GetSingleton<T>();
		
		/// <summary>
		/// 팝업 호출
		/// </summary>
		public static PopupSingleton Popup(this MonoBase mono) => GetSingleton<PopupSingleton>();
		
		/// <summary>
		/// 게임 오브젝트 비/활성화
		/// </summary>
		public static void ActiveGameObject(this Component component, bool value) => component.gameObject.SetActive(value);

		/// <summary>
		/// 컴포넌트 비/활성화
		/// </summary>
		public static void ActiveComponent(this MonoBehaviour mono, bool value) => mono.enabled = value;
		
		/// <summary>
		/// 토큰 취소 및 할당 해제
		/// </summary>
		public static void CancelAndDispose(this CancellationTokenSource cancellationTokenSource)
		{
			if (!cancellationTokenSource.IsCancellationRequested)
				cancellationTokenSource.Cancel();
		
			cancellationTokenSource.Dispose();
		}
	}
}