using System;
using System.Threading;
using System.Threading.Tasks;
using Redbean.Api;
using Redbean.Container;
using Redbean.MVP;
using Redbean.MVP.Content;
using Redbean.Singleton;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif

namespace Redbean
{
	public static partial class Extension
	{
#if UNITY_EDITOR
		/// <summary>
		/// 모델 호출
		/// </summary>
		public static T GetModel<T>(this OdinEditorWindow editor) where T : IModel => GetModel<T>();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static T GetSingleton<T>(this OdinEditorWindow editor) where T : ISingleton => GetSingleton<T>();
		
		/// <summary>
		/// API 호출
		/// </summary>
		public static async Task<Response> RequestApi<T>(this OdinEditorWindow editor, params object[] args) where T : IApi => 
			await ApiContainer.RequestApi<T>(args);
#endif
		
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
		
		/// <summary>
		/// 모델 호출
		/// </summary>
		public static T GetModel<T>(this IExtension extension) where T : IModel => GetModel<T>();
		
		/// <summary>
		/// 모델 호출
		/// </summary>
		public static object GetModel(this IExtension extension, Type type) => GetModel(type);
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static T GetSingleton<T>(this IExtension extension) where T : ISingleton => GetSingleton<T>();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static object GetSingleton(this IExtension extension, Type type) => GetSingleton(type);
		
		/// <summary>
		/// API 호출
		/// </summary>
		public static async Task<Response> RequestApi<T>(this IExtension extension, params object[] args) where T : IApi => 
			await ApiContainer.RequestApi<T>(args);
		
		/// <summary>
		/// 팝업 호출
		/// </summary>
		public static PopupSingleton Popup(this IExtension extension) => GetSingleton<PopupSingleton>();
		
		/// <summary>
		/// 유저 데이터 호출
		/// </summary>
		public static UserModel User(this IExtension extension) => GetModel<UserModel>();
		
		/// <summary>
		/// 클래스 변환
		/// </summary>
		public static T As<T>(this IExtension extension) where T : class => extension as T;

#region CancellationToken

		/// <summary>
		/// 토큰 취소 및 할당 해제
		/// </summary>
		public static void CancelAndDispose(this CancellationTokenSource cancellationTokenSource)
		{
			if (!cancellationTokenSource.IsCancellationRequested)
				cancellationTokenSource.Cancel();
		
			cancellationTokenSource.Dispose();
		}

#endregion
	}
}