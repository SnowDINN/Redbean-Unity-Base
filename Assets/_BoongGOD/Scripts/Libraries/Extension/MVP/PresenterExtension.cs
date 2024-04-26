using System;
using Redbean.Content.MVP;
using Redbean.Rx;
using Redbean.Static;

namespace Redbean.Extension
{
	public static partial class Extension
	{
		/// <summary>
		/// 모델 호출
		/// </summary>
		public static T GetModel<T>(this IPresenter presenter) where T : IModel => Model.GetOrAdd<T>();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static T GetSingleton<T>(this IPresenter presenter) where T : ISingleton => Singleton.GetOrAdd<T>();
		
		/// <summary>
		/// 유저 데이터 호출
		/// </summary>
		public static AccountModel User(this IPresenter presenter) => Model.GetOrAdd<AccountModel>();
		
		/// <summary>
		/// 로컬 데이터 호출
		/// </summary>
		public static int GetLocalInt(this IPresenter presenter, string key) =>
			GetSingleton<RxPlayerPrefsBinder>().PlayerPrefsGroup.TryGetValue(key, out var value) ? Convert.ToInt32(value) : default;
		
		/// <summary>
		/// 로컬 데이터 호출
		/// </summary>
		public static float GetLocalFloat(this IPresenter presenter, string key) =>
			GetSingleton<RxPlayerPrefsBinder>().PlayerPrefsGroup.TryGetValue(key, out var value) ? Convert.ToSingle(value) : default;
		
		/// <summary>
		/// 로컬 데이터 호출
		/// </summary>
		public static string GetLocalString(this IPresenter presenter, string key) =>
			GetSingleton<RxPlayerPrefsBinder>().PlayerPrefsGroup.TryGetValue(key, out var value) ? Convert.ToString(value) : default;

		/// <summary>
		/// 로컬 데이터 호출
		/// </summary>
		public static T GetLocalModel<T>(this IPresenter presenter, string key) =>
			GetSingleton<RxPlayerPrefsBinder>().PlayerPrefsGroup.TryGetValue(key, out var value) ? (T)value : default;
	}
}