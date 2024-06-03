using System;
using R3;
using Redbean.MVP;
using Redbean.Rx;
using Redbean.Singleton;

namespace Redbean
{
	public static partial class Extension
	{
#region MVP
		
		/// <summary>
		/// 클래스 변환
		/// </summary>
		public static T As<T>(this IMVP mvp) where T : class => mvp as T;

#endregion
		
#region Presenter
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static T GetSingleton<T>(this IPresenter mvp) where T : ISingleton => GetSingleton<T>();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static object GetSingleton(this IPresenter mvp, Type type) => GetSingleton(type);
		
		/// <summary>
		/// 모델 호출
		/// </summary>
		public static T GetModel<T>(this IPresenter mvp) where T : IModel => GetModel<T>();
		
		/// <summary>
		/// 모델 호출
		/// </summary>
		public static object GetModel(this IPresenter mvp, Type type) => GetModel(type);
		
		/// <summary>
		/// 팝업 호출
		/// </summary>
		public static PopupSingleton Popup(this IPresenter mvp) => GetSingleton<PopupSingleton>();

		public static T AddTo<T>(this T disposable, IPresenter presenter) where T : IDisposable =>
			disposable.AddTo(presenter.GetGameObject());

#endregion

#region Model

		/// <summary>
		/// 모델 데이터 배포
		/// </summary>
		public static T Publish<T>(this T model, bool isPlayerPrefs = false) where T : IModel => 
			GetSingleton<RxModelBinder>().Publish(GetSingleton<MvpSingleton>().Override(model, isPlayerPrefs));

#endregion
	}
}