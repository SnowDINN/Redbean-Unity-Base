using System;
using System.Threading.Tasks;
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
		public static FirebaseAuth Auth => FirebaseAuth.DefaultInstance;
		public static FirebaseStorage Storage => FirebaseStorage.DefaultInstance;
		public static FirebaseFirestore Firestore => FirebaseFirestore.DefaultInstance;
		
		public static DocumentReference UserDB;
		public int ExecutionOrder => 100;

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
				// 파이어스토어 앱 설정 체크
				var appSnapshot = await Firestore.Collection(FirebaseDefine.Config)
				                                 .Document(FirebaseDefine.AppConfig)
				                                 .GetSnapshotAsync();
				if (appSnapshot.Exists)
					Log.Notice("Application config load is complete.");
				else
				{
					Log.Notice("Application config load is fail.", Color.red);
					return;
				}

				var app = appSnapshot.ConvertTo<AppConfigModel>().Publish();
				if (app is not null)
				{
					var version = string.Empty;
#if UNITY_ANDROID
					version = app.Android.Version;
#elif UNITY_IOS
					version = app.iOS.Version;
#endif

					var isAppropriate = CompareVersion(version, Application.version);
					switch (isAppropriate)
					{
						// 정상 진입
						case <= 0:
						{
							var tableSnapshot = await Firestore.Collection(FirebaseDefine.Config).Document(FirebaseDefine.TableConfig).GetSnapshotAsync();
							if (tableSnapshot.Exists)
								tableSnapshot.ConvertTo<TableConfigModel>().Publish();
							
							var storageSnapshot = await Firestore.Collection(FirebaseDefine.Storage).Document(ApplicationSettings.Version).GetSnapshotAsync();
							if (storageSnapshot.Exists)
								storageSnapshot.ConvertTo<StorageFileModel>().Publish();
							
							Log.Notice($"Application is up to date. [ Latest version : {version}, Current Version : {Application.version}] ");
							break;	
						}
						
						// 업데이트 필요
						case > 0:
						{
							Log.Notice($"Application is not up to date. [ Latest version : {version}, Current Version : {Application.version} ]", Color.red);

							await TaskExtension.WaitUntil(() => false);
							break;	
						}
					}
				}
				
				Log.Success("Firebase", "Success to load to the Firebase config data.");
			}
		}

		public void Dispose()
		{
			FirebaseFirestore.DefaultInstance.ClearPersistenceAsync();
			
			FirebaseAuth.DefaultInstance.Dispose();
			FirebaseApp.DefaultInstance.Dispose();
			
			Log.System("Firebase has been terminated.");
		}
		
		private int CompareVersion(string v1, string v2)
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