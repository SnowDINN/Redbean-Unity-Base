using System;
using R3;
using Redbean.Core;
using Redbean.Dependencies;
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
		public static T GetModel<T>(this IMVP mvp) where T : Model => GetModel<T>();
		
		/// <summary>
		/// 싱글톤 호출
		/// </summary>
		public static T GetSingleton<T>(this IMVP mvp) where T : Singleton => GetSingleton<T>();
		
		/// <summary>
		/// 클래스 변환
		/// </summary>
		public static T As<T>(this IMVP mvp) where T : class, IMVP => mvp as T;

#endregion
		
#region Presenter

		public static T AddTo<T>(this T disposable, IPresenter presenter) where T : IDisposable =>
			disposable.AddTo(presenter.GetGameObject());

#endregion

#region Model

		/// <summary>
		/// 모델 데이터 배포
		/// </summary>
		public static T Publish<T>(this T model) where T : Model => 
			GetSingleton<RxModelBinder>().Publish(DependenciesModel.Override(model));

#endregion
	}
}