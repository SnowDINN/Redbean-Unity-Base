using System;
using R3;
using Redbean.MVP;
using Redbean.Rx;

namespace Redbean
{
	public static partial class Extension
	{
		/// <summary>
		/// 모델 데이터 배포
		/// </summary>
		public static T Override<T>(this T value) where T : class, IModel => 
			RxModelBinder.Publish(MvpContainer.Override(value));
		
		public static IDisposable AddTo(this IDisposable disposable, IPresenter presenter) =>
			disposable.AddTo(presenter.GetGameObject());
	}
}