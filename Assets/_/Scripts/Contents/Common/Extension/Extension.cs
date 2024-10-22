using System;
using System.Linq;
using Redbean.MVP;
using Redbean.MVP.Content;
using Redbean.Popup;
using Redbean.Table;

namespace Redbean
{
	public static partial class Extension
	{
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		private static T ExtensionGetSingleton<T>() where T : class, ISingleton => SingletonContainer.GetSingleton<T>();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		private static object ExtensionGetSingleton(Type type) => SingletonContainer.GetSingleton(type);
		
		/// <summary>
		/// 모델 호출
		/// </summary>
		private static T ExtensionGetModel<T>() where T : class, IModel => MvpContainer.GetModel<T>();
		
		/// <summary>
		/// 모델 호출
		/// </summary>
		private static object ExtensionGetModel(Type type) => MvpContainer.GetModel(type);
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static T GetSingleton<T>(this IExtension extension) where T : class, ISingleton => ExtensionGetSingleton<T>();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static object GetSingleton(this IExtension extension, Type type) => ExtensionGetSingleton(type);
		
		/// <summary>
		/// 모델 호출
		/// </summary>
		public static T GetModel<T>(this IExtension extension) where T : class, IModel => ExtensionGetModel<T>();
		
		/// <summary>
		/// 모델 호출
		/// </summary>
		public static object GetModel(this IExtension extension, Type type) => ExtensionGetModel(type);
		
		/// <summary>
		/// 팝업 호출
		/// </summary>
		public static PopupManager Popup(this IExtension extension) => ExtensionGetSingleton<PopupManager>();
		
		/// <summary>
		/// 유저 데이터 호출
		/// </summary>
		public static UserModel User(this IExtension extension) => ExtensionGetModel<UserModel>();
		
		/// <summary>
		/// 클래스 변환
		/// </summary>
		public static T As<T>(this IExtension extension) where T : class => extension as T;

		/// <summary>
		/// 언어 현지화 번역
		/// </summary>
		public static string GetLocalization(this IExtension extension, string key, params object[] args)
		{
			if (!TableContainer.Localization.TryGetValue(key, out var value))
				return key;

			var field = value.GetType().GetFields()
				.FirstOrDefault(_ => _.Name == $"{GameConfigureSetting.LanguageType}")
				.GetValue(value);
			
			return string.Format($"{field}", args);

		}
		
		/// <summary>
		/// API 호출
		/// </summary>
		private static T ExtensionGetProtocol<T>() where T : class, IApiProtocol => ApiContainer.GetProtocol<T>();
		
		/// <summary>
		/// API 호출
		/// </summary>
		private static object ExtensionGetProtocol(Type type) => ApiContainer.GetProtocol(type);
		
		/// <summary>
		/// API 호출
		/// </summary>
		public static T GetProtocol<T>(this IExtension extension) where T : class, IApiProtocol => ExtensionGetProtocol<T>();
		
		/// <summary>
		/// API 호출
		/// </summary>
		public static object GetProtocol(this IExtension extension, Type type) => ExtensionGetProtocol(type);
		
#if UNITY_EDITOR
		/// <summary>
		/// API 호출
		/// </summary>
		public static T EditorGetApi<T>(this IExtension extension) where T : class, IApiProtocol => ExtensionGetProtocol<T>();
#endif
	}
}