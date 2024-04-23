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
		private static readonly Subject<(Type type, object model)> onModelChange = new();
		public static Observable<(Type type, object model)> OnModelChange => onModelChange.Share();
		
		public RxModelBinder()
		{
			onModelChange.Subscribe(_ =>
			{
				Console.Log("Model", $"Update model : {_.type.FullName}", Color.yellow);
			}).AddTo(disposables);
		}

		~RxModelBinder()
		{
			Dispose();
		}

		public T Publish<T>(T value)
		{
			onModelChange.OnNext((value.GetType(), value));
			return value;
		}
	}
}