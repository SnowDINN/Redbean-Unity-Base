using System;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Redbean.Api;
using Redbean.Popup.Content;
using Redbean.Rx;
using UnityEngine;

namespace Redbean
{
	public class OnStartBootstrap : Bootstrap
	{
		protected override async Task Setup()
		{
			Application.logMessageReceived += OnLogMessageReceived;

			AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(x => x.GetTypes())
				.Where(x => typeof(RxBase).IsAssignableFrom(x)
				            && typeof(RxBase).FullName != x.FullName
				            && !x.IsInterface
				            && !x.IsAbstract)
				.ToList()
				.ForEach(_ => (Activator.CreateInstance(_) as RxBase).Start());
			
			// 파이어베이스 연결 체크
			var status = await FirebaseApp.CheckAndFixDependenciesAsync();
			if (status == DependencyStatus.Available)
				Log.Success("FIREBASE", "Success to connect to the Firebase server.");
			else
			{
				Log.Fail("FIREBASE", "Failed to connect to the Firebase server.");
				return;
			}

			// 앱 설정 체크
			await this.GetProtocol<GetAppSettingProtocol>().RequestAsync(cancellationToken);
			await this.GetProtocol<GetTableSettingProtocol>().RequestAsync(cancellationToken);
		}

		protected override Task Teardown()
		{
			Application.logMessageReceived -= OnLogMessageReceived;
			
			FirebaseAuth.DefaultInstance.Dispose();
			FirebaseApp.DefaultInstance.Dispose();

			return Task.CompletedTask;
		}

		private void OnLogMessageReceived(string condition, string stacktrace, LogType type)
		{
			if (type != LogType.Exception)
				return;
			
			this.Popup().AssetOpen<PopupException>().ExceptionMessage = condition;
		}
	}
}