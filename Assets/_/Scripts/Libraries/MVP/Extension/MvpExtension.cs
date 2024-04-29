using System;
using R3;
using Redbean.Content.MVP;
using Redbean.Core;
using Redbean.MVP;
using Redbean.Rx;

namespace Redbean
{
	public static partial class Extension
	{
#region MVP

		/// <summary>
		/// 모델 호출
		/// </summary>
		public static T GetModel<T>(this IMVP mvp) where T : IModel => Model.GetOrAdd<T>();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static T GetSingleton<T>(this IMVP mvp) where T : ISingleton => Singleton.GetOrAdd<T>();

#endregion
		
#region Presenter

		public static T AddTo<T>(this T disposable, IPresenter presenter) where T : IDisposable =>
			disposable.AddTo(presenter.GetGameObject());
		
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

#endregion

#region Model

		/// <summary>
		/// 모델 데이터 배포
		/// </summary>
		public static T Publish<T>(this T model) where T : IModel => 
			model is not IModel ? default : Singleton.GetOrAdd<RxModelBinder>().Publish(Model.Add(model));

#endregion
	}
}