using System.Threading;
using Redbean.Static;
using UnityEngine;

namespace Redbean.Extension
{
	public static partial class Extension
	{
		/// <summary>
		/// 게임 오브젝트 비/활성화
		/// </summary>
		public static void ActiveGameObject(this Component component, bool value) => 
			component.gameObject.SetActive(value);

		/// <summary>
		/// 컴포넌트 비/활성화
		/// </summary>
		public static void ActiveComponent(this MonoBehaviour mono, bool value) => 
			mono.enabled = value;
		
		/// <summary>
		/// 모델 호출
		/// </summary>
		public static T GetModel<T>(this MonoBehaviour mono) where T : IModel => Model.GetOrAdd<T>();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static T GetSingleton<T>(this MonoBehaviour mono) where T : ISingleton => Singleton.GetOrAdd<T>();
		
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