using System;
using Cysharp.Threading.Tasks;
using R3;
using Redbean.Api;

namespace Redbean.Rx
{
	public class RxApiBinder : RxBase
	{
		private readonly Subject<(Type type, object response)> onApiResponse = new();
		private Observable<(Type type, object response)> OnApiResponse => onApiResponse.Share();

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

		public void Publish<T>(object response) where T : IApiContainer
		{
			onApiResponse.OnNext((typeof(T), response));
		}
	}
}