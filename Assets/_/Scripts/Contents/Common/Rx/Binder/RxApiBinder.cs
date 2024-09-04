using System;
using Cysharp.Threading.Tasks;
using R3;
using Redbean.Api;

namespace Redbean.Rx
{
	public class RxApiBinder : RxBase
	{
		private static readonly Subject<Type> onApiRequest = new();
		public static Observable<Type> OnApiRequest => onApiRequest.Share();
		
		private static readonly Subject<(Type type, ApiResponse response)> onApiResponse = new();
		public static Observable<(Type type, ApiResponse response)> OnApiResponse => onApiResponse.Share();

		public override void Setup()
		{
			base.Setup();
			
			Observable.Interval(TimeSpan.FromSeconds(60))
				.Where(_ => ApiAuthentication.IsRefreshTokenExist && ApiAuthentication.IsAccessTokenExpired)
				.Subscribe(_ => UniTask.Void(GetRefreshAccessTokenAsync))
				.AddTo(disposables);

			ApiContainer.OnRequest += OnRequest;
			ApiContainer.OnResponse += OnResponse;
		}

		public override void Teardown()
		{
			base.Teardown();
			
			ApiContainer.OnRequest -= OnRequest;
			ApiContainer.OnResponse -= OnResponse;
		}

		private void OnRequest(Type type) => onApiRequest.OnNext(type);

		private void OnResponse(Type type, ApiResponse response) => onApiResponse.OnNext((type, response));

		private async UniTaskVoid GetRefreshAccessTokenAsync() => 
			await this.GetProtocol<PostAccessTokenRefreshProtocol>().RequestAsync(AppLifeCycle.AppCancellationToken);
	}
}