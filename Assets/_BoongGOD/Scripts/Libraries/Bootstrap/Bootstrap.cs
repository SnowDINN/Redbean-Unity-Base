using Firebase;
using Firebase.Firestore;
using R3;
using Redbean.Content.Model;
using Redbean.Define;
using Redbean.Extension;
using Redbean.Static;
using UnityEngine;

namespace Redbean
{
	public class Bootstrap
	{
		private static readonly Subject<AppStartedType> onAppStarted = new();
		public static Observable<AppStartedType> OnAppStarted => onAppStarted.Share();

		private static Singleton Singleton;
		private static Model Model;
		
		[RuntimeInitializeOnLoadMethod]
		public static async void Setup()
		{
			Singleton = new Singleton();
			Model = new Model();

			var status = await FirebaseApp.CheckAndFixDependenciesAsync();
			if (status == DependencyStatus.Available)
				Console.Log("Firebase", "Success to connect to the Firebase server.", Color.green);
			else
			{
				onAppStarted.OnNext(AppStartedType.FirebaseError);
				onAppStarted.OnCompleted();
				Console.Log("Firebase", "Failed to connect to the Firebase server.", Color.red);
				return;
			}

			var firestore = FirebaseFirestore.DefaultInstance;
			var configSnapshot = await firestore.Collection("app_config").Document("setup").GetSnapshotAsync();
			if (configSnapshot.Exists)
				Console.Log("Firebase", "Success to load to the app config.", Color.green);
			else
			{
				onAppStarted.OnNext(AppStartedType.FirebaseError);
				onAppStarted.OnCompleted();
				Console.Log("Firebase", "Failed to load to the app config.", Color.red);
				return;
			}

			var config = configSnapshot.ConvertTo<AppConfigModel>().AddModel();
			if (config is not null)
			{
#if UNITY_ANDROID
				AppConfigSettings(config.android);
#elif UNITY_IOS
				AppConfigSettings(config.ios);
#endif
			}
		}

		private static void AppConfigSettings(AppConfigArgument configArgs)
		{
			Console.Log("App Config", $"Latest updated version : {configArgs.version}", Color.yellow);
		}
	}   
}