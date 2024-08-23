using System;
using Cysharp.Threading.Tasks;
using R3;
using Redbean.Api;

namespace Redbean.Rx
{
	public class RxApiBinder : RxBase, ISingletonContainer
	{
		private readonly Subject<(Type type, object response)> onApiResponse = new();
		private Observable<(Type type, object response)> OnApiResponse => onApiResponse.Share();

		public RxApiBinder()
		{
			Observable.Interval(TimeSpan.FromSeconds(60))
				.Where(_ => ApiAuthentication.IsRefreshTokenExist && ApiAuthentication.IsAccessTokenExpired)
				.Subscribe(_ => { UniTask.Void(GetRefreshAccessTokenAsync); })
				.AddTo(disposables);
		}

		private async UniTaskVoid GetRefreshAccessTokenAsync()
		{
			await this.GetProtocol<GetAccessTokenRefreshProtocol>().RequestAsync(AppLifeCycle.AppCancellationToken);
		}

		public void Publish<T>(object response) where T : ApiProtocol
		{
			onApiResponse.OnNext((typeof(T), response));
		}
	}
}