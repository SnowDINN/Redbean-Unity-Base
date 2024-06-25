using System;
using Cysharp.Threading.Tasks;
using R3;
using Redbean.Api;

namespace Redbean.Rx
{
	public class RxApiBinder : RxBase
	{
		private readonly Subject<(Type type, Response response)> onApiResponse = new();
		private Observable<(Type type, Response response)> OnApiResponse => onApiResponse.Share();

		public RxApiBinder()
		{
			Observable.Interval(TimeSpan.FromSeconds(60))
				.Where(_ => ApiContainer.IsRefreshTokenExist && ApiContainer.IsAccessTokenExpired)
				.Subscribe(_ => { UniTask.Void(GetRefreshAccessTokenAsync); });
		}

		private async UniTaskVoid GetRefreshAccessTokenAsync()
		{
			await this.RequestApi<GetRefreshAccessTokenProtocol>();
		}

		public void Publish<T>(Response response) where T : IApi
		{
			onApiResponse.OnNext((typeof(T), response));
		}
	}
}