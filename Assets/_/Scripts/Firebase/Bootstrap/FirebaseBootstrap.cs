﻿using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Redbean.MVP.Content;
using UnityEngine;

namespace Redbean.Firebase
{
	public class FirebaseBootstrap : IApplicationBootstrap
	{
		public static FirebaseAuth Auth;
		public static FirebaseFirestore Firestore;
		public static DocumentReference UserDB;
		public int ExecutionOrder => 100;

		public async UniTask Setup()
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
				// 파이어스토어 앱 설정 체크
				Auth = FirebaseAuth.DefaultInstance;
				Firestore = FirebaseFirestore.DefaultInstance;
				
				var configSnapshot = await Firestore.Collection("app_config").Document("setup").GetSnapshotAsync();
				if (configSnapshot.Exists)
					Log.Success("Firebase", "Success to load to the app config.");
				else
				{
					Log.Fail("Firebase", "Failed to load to the app config.");
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
		}

		public void Dispose()
		{
			FirebaseApp.DefaultInstance.Dispose();
			FirebaseAuth.DefaultInstance.Dispose();
			
			Log.System("Firebase has been terminated.");
		}

		private static void AppConfigSettings(MobileConfigArgument configArgs)
		{

		}
	}
}