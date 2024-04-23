using System;
using System.Collections.Generic;
using System.Linq;
using Firebase;
using Firebase.Firestore;
using R3;
using Redbean.Content.Model;
using Redbean.Define;
using Redbean.Extension;
using UnityEngine;
using Console = Redbean.Extension.Console;

namespace Redbean
{
	public class Bootstrap
	{
		private static readonly Subject<AppStartedType> onAppStarted = new();
		public static Observable<AppStartedType> OnAppStarted => onAppStarted.Share();
		
		private static readonly Dictionary<string, IBootstrap> Instances = new();
		
		[RuntimeInitializeOnLoadMethod]
		public static async void Setup()
		{
			var instances = AppDomain.CurrentDomain.GetAssemblies()
			                         .SelectMany(x => x.GetTypes())
			                         .Where(x => typeof(IBootstrap).IsAssignableFrom(x)
			                                     && !x.IsInterface
			                                     && !x.IsAbstract)
			                         .Select(x => (IBootstrap)Activator.CreateInstance(Type.GetType(x.FullName)));

			foreach (var instance in instances
				         .Where(singleton => Instances.TryAdd(singleton.GetType().Name, singleton)))
				Console.Log("Bootstrap", $" Success create instance {instance.GetType().FullName}", Color.green);

			// 파이어베이스 연결 체크
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

			// 파이어스토어 앱 설정 체크
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

			var config = configSnapshot.ConvertTo<AppConfigModel>().Override();
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