using System.Threading;
using Redbean.Static;
using UnityEngine;

namespace Redbean.Extension
{
	public static partial class Extension
	{
		private static T GetSingleton<T>() where T : ISingleton => Singleton.GetOrAdd<T>();
		
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