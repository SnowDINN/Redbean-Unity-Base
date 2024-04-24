using System;
using R3;
using Redbean.Base;
using Redbean.Static;
using UnityEngine;
using Console = Redbean.Extension.Console;

namespace Redbean.Rx
{
	public class RxModelBinder : RxBase, ISingleton
	{
		private readonly Subject<(Type type, object value)> onModelChanged = new();
		public Observable<(Type type, object value)> OnModelChanged => onModelChanged.Share();
		
		public RxModelBinder()
		{
			OnModelChanged.Subscribe(_ =>
			{
				Console.Log("Model", $"Published model : {_.type.FullName}", Color.yellow);
			}).AddTo(disposables);

			OnModelChanged.Where(_ => _.type.IsAssignableFrom(typeof(IPostModel)))
			              .Select(_ => (IPostModel)_.value)
			              .Subscribe(_ =>
			              {

			              }).AddTo(disposables);
		}

		~RxModelBinder()
		{
			Dispose();
		}

		public T Publish<T>(T value)
		{
			onModelChanged.OnNext((value.GetType(), value));
			return value;
		}
	}
}