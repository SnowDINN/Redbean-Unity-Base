using System;
using R3;
using Redbean.Base;
using Redbean.Debug;
using Redbean.Static;
using UnityEngine;

namespace Redbean.Rx
{
	public class RxModelBinder : RxBase
	{
		private readonly Subject<(Type type, object value)> onModelChanged = new();
		public Observable<(Type type, object value)> OnModelChanged => onModelChanged.Share();
		
		public RxModelBinder()
		{
			OnModelChanged.Subscribe(_ =>
			{
				Log.Print("Model", $"Published model : {_.type.FullName}", Color.yellow);
			}).AddTo(disposables);

			OnModelChanged.Where(_ => _.type.IsAssignableFrom(typeof(IPostModel)))
			              .Select(_ => (IPostModel)_.value)
			              .Subscribe(_ =>
			              {

			              }).AddTo(disposables);
		}

		public T Publish<T>(T value)
		{
			onModelChanged.OnNext((value.GetType(), value));
			return value;
		}
	}
}