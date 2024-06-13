using System;
using R3;
using Redbean.Api;

namespace Redbean.Rx
{
	public class RxApiBinder : RxBase
	{
		private readonly Subject<(Type type, Response response)> onApiResponse = new();
		private Observable<(Type type, Response response)> OnApiResponse => onApiResponse.Share();

		public void OnNext<T>(Response response) where T : IApi
		{
			onApiResponse.OnNext((typeof(T), response));
		}
	}
}