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
				.Subscribe(_ => { UniTask.Void(GetRefreshAccessTokenAsync); })
				.AddTo(disposables);
		}
		
		public static void OnRequestPublish<T>() where T : ApiProtocol
		{
			onApiRequest.OnNext(typeof(T));
		}
		
		public static void OnRequestPublish(Type type)
		{
			onApiRequest.OnNext(type);
		}

		public static void OnResponsePublish<T>(ApiResponse response) where T : ApiProtocol
		{
			onApiResponse.OnNext((typeof(T), response));
		}
		
		public static void OnResponsePublish(Type type, ApiResponse response)
		{
			onApiResponse.OnNext((type, response));
		}
		
		private async UniTaskVoid GetRefreshAccessTokenAsync()
		{
			await this.GetProtocol<PostAccessTokenRefreshProtocol>().RequestAsync(AppLifeCycle.AppCancellationToken);
		}
	}
}