using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Firestore;
using Redbean.Core;
using Redbean.Debug;
using Redbean.MVP.Content;
using UnityEngine;

namespace Redbean.Firebase
{
	public class FirebaseCore : IApplicationStarted
	{
		public static FirebaseFirestore Firestore;
		public static DocumentReference UserDB;
		public int ExecutionOrder => 10;

		public async UniTask Setup()
		{
			// 파이어베이스 연결 체크
			var status = await FirebaseApp.CheckAndFixDependenciesAsync();
			if (status == DependencyStatus.Available)
				Log.Print("Firebase", "Success to connect to the Firebase server.", Color.green);
			else
			{
				Log.Print("Firebase", "Failed to connect to the Firebase server.", Color.red);
				return;
			}

			// 파이어스토어 앱 설정 체크
			Firestore = FirebaseFirestore.DefaultInstance;
			var configSnapshot = await Firestore.Collection("app_config").Document("setup").GetSnapshotAsync();
			if (configSnapshot.Exists)
				Log.Print("Firebase", "Success to load to the app config.", Color.green);
			else
			{
				Log.Print("Firebase", "Failed to load to the app config.", Color.red);
				return;
			}

			var config = configSnapshot.ConvertTo<AppConfigModel>().Publish();
			if (config is not null)
			{
#if UNITY_ANDROID
				AppConfigSettings(config.Android);
#elif UNITY_IOS
				AppConfigSettings(config.iOS);
#endif
			}
		}

		private static void AppConfigSettings(AppConfigArgument configArgs)
		{
			Log.Print("Config", $"Latest updated version : {configArgs.Version}", Color.yellow);
		}
	}
}