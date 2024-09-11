using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Redbean.Api;

namespace Redbean
{
	public class OnValidationBootstrap : Bootstrap
	{
		protected override async Task Setup()
		{
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
			FirebaseAuth.DefaultInstance.Dispose();
			FirebaseApp.DefaultInstance.Dispose();

			return Task.CompletedTask;
		}
	}
}