using System;
using Cysharp.Threading.Tasks;
using R3;
using Redbean.Api;

namespace Redbean.Rx
{
	public class RxApiBinder : RxBase
	{
		private static readonly Subject<Type> onRequest = new();
		public static Observable<Type> OnRequest => onRequest.Share();
		
		private static readonly Subject<(Type type, ApiResponse response)> onResponse = new();
		public static Observable<(Type type, ApiResponse response)> OnResponse => onResponse.Share();

		public override void Setup()
		{
			base.Setup();
			
			Observable.Interval(TimeSpan.FromSeconds(60))
				.Where(_ => ApiAuthentication.IsRefreshTokenExist && ApiAuthentication.IsAccessTokenExpired)
				.Subscribe(_ => UniTask.Void(GetRefreshAccessTokenAsync))
				.AddTo(disposables);

			ApiContainer.OnRequest += OnApiRequest;
			ApiContainer.OnResponse += OnApiResponse;
		}

		public override void Teardown()
		{
			base.Teardown();
			
			ApiContainer.OnRequest -= OnApiRequest;
			ApiContainer.OnResponse -= OnApiResponse;
		}

		private void OnApiRequest(Type type) => onRequest.OnNext(type);

		private void OnApiResponse(Type type, ApiResponse response) => onResponse.OnNext((type, response));

		private async UniTaskVoid GetRefreshAccessTokenAsync() => 
			await this.GetProtocol<PostAccessTokenRefreshProtocol>().RequestAsync(cancellationToken);
	}
}