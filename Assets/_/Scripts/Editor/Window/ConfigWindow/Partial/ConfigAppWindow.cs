using System;
using System.Linq;
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
		private const string TableTitle = "Table";
		private const string VersionTitle = "Version";

		[TabGroup(ConfigTab), Title(TableTitle), LabelText("Path"), ShowInInspector, FolderPath]
		public string TablePath
		{
			get => googleTable.Path;
			set
			{
				googleTable.Path = value;
				googleTable.Save();
			}
		}

		[TabGroup(ConfigTab), LabelText("Item Path"), ShowInInspector, FolderPath]
		public string TableItemPath
		{
			get => googleTable.ItemPath;
			set
			{
				googleTable.ItemPath = value;
				googleTable.Save();
			}
		}
		
		[TabGroup(ConfigTab), Button("UPDATE ALL TABLE", ButtonSizes.Large)]
		private async void UpdateAllTable()
		{
			EditorUtility.DisplayProgressBar("Table Update Progress Bar", "Doing some work...", 0);
			{
				using var container = new DataContainer();
				await container.Setup();
				
				using var firebase = new FirebaseBootstrap();
				await firebase.Setup();
			
				DataContainer.Override((await GetAppConfig()).Model);
			
				var sheetRaw = await GoogleTableGenerator.GetSheetRaw();
				await GoogleTableGenerator.GenerateCSharp(sheetRaw);

				var keys = sheetRaw.Keys.ToArray();
				var values = sheetRaw.Values.ToArray();
				for (var i = 0; i < sheetRaw.Count; i++)
				{
					EditorUtility.DisplayProgressBar("Table Update Progress Bar", "Doing some work...", i + 1 / sheetRaw.Count);
					await GoogleTableGenerator.GenerateItemCSharp(keys[i], values[i]);
				}
			}
			EditorUtility.ClearProgressBar();
			
			Log.Notice("Table update is complete.");
			
			AssetDatabase.Refresh();
		}
		
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

		private static async UniTask<(DocumentReference Document, AppConfigModel Model)> GetAppConfig()
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
		
		[Serializable]
		internal class Toggle
		{
			public bool Enabled;
		}
	}
}