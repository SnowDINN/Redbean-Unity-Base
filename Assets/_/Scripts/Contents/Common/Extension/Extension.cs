﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Redbean.Api;
using Redbean.MVP;
using Redbean.MVP.Content;
using Redbean.Singleton;

namespace Redbean
{
	public static partial class Extension
	{
#if UNITY_EDITOR
		/// <summary>
		/// API 호출
		/// </summary>
		public static async Task<Response> EditorRequestApi<T>(this IExtension extension, params object[] args) where T : IApiContainer => 
			await ApiContainer.RequestApi<T>(args);
#endif
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		private static T GetSingleton<T>() where T : ISingletonContainer => SingletonContainer.GetSingleton<T>();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		private static object GetSingleton(Type type) => SingletonContainer.GetSingleton(type);
		
		/// <summary>
		/// 모델 호출
		/// </summary>
		private static T GetModel<T>() where T : IModel => SingletonContainer.GetSingleton<MvpSingletonContainer>().GetModel<T>();
		
		/// <summary>
		/// 모델 호출
		/// </summary>
		private static object GetModel(Type type) => SingletonContainer.GetSingleton<MvpSingletonContainer>().GetModel(type);
		
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
		public static T GetSingleton<T>(this IExtension extension) where T : ISingletonContainer => GetSingleton<T>();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static object GetSingleton(this IExtension extension, Type type) => GetSingleton(type);
		
		/// <summary>
		/// API 호출
		/// </summary>
		public static async Task<Response> RequestApi<T>(this IExtension extension, params object[] args) where T : IApiContainer
		{
			T generic = default;
			
			var request = await ApiContainer.RequestApi<T>(args);
			ApiPublish(generic, request);
			
			return request;
		}

		/// <summary>
		/// 팝업 호출
		/// </summary>
		public static PopupSingletonContainer Popup(this IExtension extension) => GetSingleton<PopupSingletonContainer>();
		
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