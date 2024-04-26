using System;
using System.Collections.Generic;
using System.Linq;
using Firebase;
using Firebase.Firestore;
using R3;
using Redbean.Content.MVP;
using Redbean.Define;
using Redbean.Extension;
using UnityEngine;

namespace Redbean
{
	public class Bootstrap
	{
		private static readonly Subject<AppStartedType> onAppStarted = new();
		public static Observable<AppStartedType> OnAppStarted => onAppStarted.Share();
		
		private static readonly Dictionary<string, IBootstrap> Instances = new();
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
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
				Log.Print("System", $" Success create instance {instance.GetType().FullName}", Color.green);

			// 파이어베이스 연결 체크
			var status = await FirebaseApp.CheckAndFixDependenciesAsync();
			if (status == DependencyStatus.Available)
				Log.Print("Firebase", "Success to connect to the Firebase server.", Color.green);
			else
			{
				onAppStarted.OnNext(AppStartedType.FirebaseError);
				onAppStarted.OnCompleted();
				Log.Print("Firebase", "Failed to connect to the Firebase server.", Color.red);
				return;
			}

			// 파이어스토어 앱 설정 체크
			var firestore = FirebaseFirestore.DefaultInstance;
			var configSnapshot = await firestore.Collection("app_config").Document("setup").GetSnapshotAsync();
			if (configSnapshot.Exists)
				Log.Print("Firebase", "Success to load to the app config.", Color.green);
			else
			{
				onAppStarted.OnNext(AppStartedType.FirebaseError);
				onAppStarted.OnCompleted();
				Log.Print("Firebase", "Failed to load to the app config.", Color.red);
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
			
			onAppStarted.OnNext(AppStartedType.Success);
		}

		private static void AppConfigSettings(AppConfigArgument configArgs)
		{
			Log.Print("Config", $"Latest updated version : {configArgs.version}", Color.yellow);
		}
	}   
}