using System.Threading;
using Redbean.Dependencies;
using Redbean.MVP;
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
		private static T GetSingleton<T>() where T : ISingleton => DependenciesSingleton.GetOrAdd<T>();
		
		/// <summary>
		/// 모델 호출
		/// </summary>
		private static T GetModel<T>() where T : IModel => DependenciesModel.GetOrAdd<T>();
		
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