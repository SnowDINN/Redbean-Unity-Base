using System.Threading;
using Redbean.Static;

namespace Redbean.Extension
{
	public static partial class Extension
	{
		/// <summary>
		/// 토큰 취소 및 할당 해제
		/// </summary>
		public static void CancelAndDispose(this CancellationTokenSource cancellationTokenSource)
		{
			if (!cancellationTokenSource.IsCancellationRequested)
				cancellationTokenSource.Cancel();
		
			cancellationTokenSource.Dispose();
		}
		
		/// <summary>
		/// 모델 호출
		/// </summary>
		public static T GetModel<T>(this IPresenter presenter) where T : IModel => Model.Get<T>();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static T GetSingleton<T>(this IPresenter presenter) where T : ISingleton => Singleton.Get<T>();
		
		/// <summary>
		/// 모델 호출
		/// </summary>
		public static T GetModel<T>(this IModel model) where T : IModel => Model.Get<T>();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static T GetSingleton<T>(this IModel model) where T : ISingleton => Singleton.Get<T>();
	}
}