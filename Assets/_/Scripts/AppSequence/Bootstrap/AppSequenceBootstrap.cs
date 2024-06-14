using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Redbean.Api;
using Redbean.MVP.Content;
using UnityEngine;

namespace Redbean.Firebase
{
	public class AppSequenceBootstrap : IAppBootstrap
	{
		public int ExecutionOrder => 1;

		public async Task Setup()
		{
			// 파이어베이스 연결 체크
			var status = await FirebaseApp.CheckAndFixDependenciesAsync();
			if (status == DependencyStatus.Available)
				Log.Success("Firebase", "Success to connect to the Firebase server.");
			else
			{
				Log.Fail("Firebase", "Failed to connect to the Firebase server.");
				return;
			}

			if (Application.isPlaying)
			{
				// 앱 설정 체크
				var request = await this.RequestApi<GetAppConfigProtocol>();
				var app = new AppConfigModel
				{
					Response = request.ToConvert<AppConfigResponse>()
				}.Publish();
				
				if (app is not null)
				{
					var version = string.Empty;
#if UNITY_ANDROID
					version = app.Response.Android.Version;
#elif UNITY_IOS
					version = app.Response.iOS.Version;
#endif

					var isAppropriate = CompareVersion(version, Application.version);
					switch (isAppropriate)
					{
						// 정상 진입
						case <= 0:
						{
							// TODO : 정상 진입 로직
							break;	
						}
						
						// 업데이트 필요
						case > 0:
						{
							// TODO : 업데이트 진입 로직

							await TaskExtension.WaitUntil(() => false);
							break;	
						}
					}
				}
			}
		}

		public void Dispose()
		{
			FirebaseAuth.DefaultInstance.Dispose();
			FirebaseApp.DefaultInstance.Dispose();
			
			Log.System("Firebase has been terminated.");
		}
		
		private static int CompareVersion(string v1, string v2)
		{
			var v1a = v1.Split('.', StringSplitOptions.RemoveEmptyEntries);
			var v2a = v2.Split('.', StringSplitOptions.RemoveEmptyEntries);

			if (v1a.Length == 0 || v2a.Length == 0)
				return -1;
			
			var maxLength = v1a.Length > v2a.Length ? v1a.Length : v2a.Length;
			for (var i = 0; i < maxLength; i++)
			{
				int v1i = 0, v2i = 0;
				if (v1a.Length > i && !int.TryParse(v1a[i], out v1i)) 
					return -1;
				
				if (v2a.Length > i && !int.TryParse(v2a[i], out v2i))
					return -1;
				
				if (v1i != v2i) 
					return v1i.CompareTo(v2i);
			}

			return 0;
		}
	}
}