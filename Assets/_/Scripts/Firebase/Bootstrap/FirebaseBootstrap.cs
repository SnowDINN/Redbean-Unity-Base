using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Storage;
using Redbean.MVP.Content;
using UnityEngine;

namespace Redbean.Firebase
{
	public class FirebaseBootstrap : IApplicationBootstrap
	{
		public static FirebaseAuth Auth;
		public static FirebaseStorage Storage;
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
				Storage = FirebaseStorage.DefaultInstance;
				Firestore = FirebaseFirestore.DefaultInstance;
				
				var appSnapshot = await Firestore.Collection("config").Document("app").GetSnapshotAsync();
				if (appSnapshot.Exists)
					Log.Success("Firebase", "Success to load to the app config.");
				else
				{
					Log.Fail("Firebase", "Failed to load to the app config.");
					return;
				}

				var app = appSnapshot.ConvertTo<AppConfigModel>().Publish();
				if (app is not null)
				{
					// 앱 설정 관련
				}
				
				var tableSnapshot = await Firestore.Collection("config").Document("table").GetSnapshotAsync();
				if (tableSnapshot.Exists)
					Log.Success("Firebase", "Success to load to the table config.");
				else
				{
					Log.Fail("Firebase", "Failed to load to the table config.");
					return;
				}

				var table = tableSnapshot.ConvertTo<TableConfigModel>().Publish();
				if (table is not null)
				{
					// 테이블 설정 관련
				}
			}
		}

		public void Dispose()
		{
			FirebaseAuth.DefaultInstance.Dispose();
			FirebaseApp.DefaultInstance.Dispose();
			
			FirebaseFirestore.DefaultInstance.ClearPersistenceAsync();
			
			Log.System("Firebase has been terminated.");
		}
	}
}