using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Firestore;
using Redbean.Dependencies;
using Redbean.Firebase;
using Redbean.MVP.Content;
using Redbean.Table;
using Sirenix.OdinInspector;
using UnityEditor;

namespace Redbean.Editor
{
	internal partial class ApplicationWindow
	{
		private const string VersionTitle = "Version";
		private const string TableTitle = "Table";
		
		[TabGroup(ConfigTab), Title(VersionTitle), Button("Android")]
		private async void AndroidVersion(string version = "0.0.1")
		{
			using var core = new FirebaseBootstrap();
			await core.Setup();
			
			var config = await GetAppConfig();
			var before = config.Model.Android.Version;
			config.Model.Android.Version = version;

			await config.Document.SetAsync(config.Model);
			
			FirebaseApp.DefaultInstance.Dispose();
			
			Log.Notice($"Android version changed from {before} -> {version}.");
		}
		
		[TabGroup(ConfigTab), Button("iOS")]
		private async void IosVersion(string version = "0.0.1")
		{
			using var core = new FirebaseBootstrap();
			await core.Setup();
			
			var config = await GetAppConfig();
			var before = config.Model.iOS.Version;
			config.Model.iOS.Version = version;

			await config.Document.SetAsync(config.Model);
			
			FirebaseApp.DefaultInstance.Dispose();
			
			Log.Notice($"Android version changed from {before} -> {version}.");
		}

		[TabGroup(ConfigTab), Title(TableTitle), Button("UPDATE TABLE")]
		private async void UpdateTable()
		{
			using var container = new DataContainer();
			await container.Setup();
			
			using var firebase = new FirebaseBootstrap();
			await firebase.Setup();
			
			DataContainer.Override((await GetAppConfig()).Model);
			
			var sheetRaw = await GoogleTableGenerator.GetSheetRaw();
			await GoogleTableGenerator.GenerateCSharp(sheetRaw);
			foreach (var table in sheetRaw)
				await GoogleTableGenerator.GenerateItemCSharp(table.Key, table.Value);
			
			AssetDatabase.Refresh();
		}

		private async UniTask<(DocumentReference Document, AppConfigModel Model)> GetAppConfig()
		{
			var document = FirebaseFirestore.DefaultInstance.Collection("app_config").Document("setup");
			var snapshotAsync = await document.GetSnapshotAsync();
			if (snapshotAsync.Exists)
				Log.Success("Firebase", "Success to load to the app config.");
			else
			{
				Log.Fail("Firebase", "Failed to load to the app config.");
				return default;
			}
				
			return (document, snapshotAsync.ConvertTo<AppConfigModel>());
		}
	}
}