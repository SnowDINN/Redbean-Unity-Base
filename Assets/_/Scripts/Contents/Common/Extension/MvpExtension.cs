using System;
using R3;
using Redbean.MVP;

namespace Redbean
{
	public static partial class Extension
	{
#region Presenter
		
		public static IDisposable AddTo(this IDisposable disposable, IPresenter presenter) =>
			disposable.AddTo(presenter.GetGameObject());

#endregion
	}
}