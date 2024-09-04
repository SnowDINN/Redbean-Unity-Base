using System;
using R3;
using Redbean.MVP;
using Redbean.Rx;
using Redbean.Singleton;

namespace Redbean
{
	public static partial class Extension
	{
		/// <summary>
		/// 모델 데이터 배포
		/// </summary>
		public static T Override<T>(this T model) where T : IModel => 
			RxModelBinder.Publish(MvpContainer.Override(model));
		
		public static IDisposable AddTo(this IDisposable disposable, IPresenter presenter) =>
			disposable.AddTo(presenter.GetGameObject());
	}
}