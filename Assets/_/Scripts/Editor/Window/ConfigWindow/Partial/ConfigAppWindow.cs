using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase;
using Firebase.Firestore;
using Firebase.Storage;
using Redbean.Bundle;
using Redbean.Dependencies;
using Redbean.Firebase;
using Redbean.MVP.Content;
using Redbean.Table;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace Redbean.Editor
{
	internal partial class ApplicationWindow
	{
		private const string BundleTitle = "Bundle";
		private const string TableTitle = "Table";
		private const string VersionTitle = "Version";

		private const int BundleOrder = 0;
		private const int TableOrder = 1;
		private const int VersionOrder = 2;
		
		[TabGroup(ConfigTab), Title(BundleTitle), PropertyOrder(BundleOrder), Button("UPDATE BUNDLE", ButtonSizes.Large)]
		private async void UpdateBundle()
		{
			using var firebase = new FirebaseBootstrap();
			await firebase.Setup();
			
			var result = AddressableGenerator.TryBuildBundle();
			var buildPath = AddressableAssetSettingsDefaultObject.Settings.RemoteCatalogBuildPath.GetValue(AddressableAssetSettingsDefaultObject.Settings);
			var buildFiles = result.FileRegistry.GetFilePaths().Where(_ => _.Contains(buildPath)).ToArray();

			try
			{
				EditorUtility.DisplayProgressBar("Bundle Update", "Updating Bundle...", 0);
				
				var storage = await GetStorageFiles();
				foreach (var file in storage.Model.Bundle)
					await FirebaseBootstrap.Storage.GetReference(StoragePath.BundleRequest(file)).DeleteAsync();

				var path = buildFiles.Select(_ => _.Replace("\\", "/")).ToArray();
				for (var i = 0; i < path.Length; i++)
				{
					var filename = path[i].Split('/').Last();
					EditorUtility.DisplayProgressBar("Bundle Update", $"Updating {filename} Bundle...", (i + 1) / (float)path.Length);
					var storageReference = FirebaseBootstrap.Storage.GetReference(path[i]);
					var metadata = new MetadataChange
					{
						CacheControl = "no-store",
					};
				
					await storageReference.PutFileAsync($"{Application.dataPath.Replace("Assets", "")}{path[i]}", metadata);
					Log.Notice($"{filename} Bundle update is complete.");
				}
			
				storage.Model.Bundle = path.Select(_ => _.Split('/').Last()).ToArray();
				await storage.Document.SetAsync(storage.Model);
			}
			catch (Exception e)
			{
				Log.Fail("Error", e.Message);
				EditorUtility.ClearProgressBar();
			}
			
			Log.Success("Table", "Success to update to the Bundles.");
			EditorUtility.ClearProgressBar();
		}

		[TabGroup(ConfigTab), Title(TableTitle), PropertyOrder(TableOrder), LabelText("Path"), ShowInInspector, FolderPath]
		private string TablePath
		{
			get => GoogleTableSettings.Path;
			set
			{
				GoogleTableSettings.Path = value;
				GoogleTableSettings.Save();
			}
		}

		[TabGroup(ConfigTab), PropertyOrder(TableOrder), LabelText("Item Path"), ShowInInspector, FolderPath]
		private string TableItemPath
		{
			get => GoogleTableSettings.ItemPath;
			set
			{
				GoogleTableSettings.ItemPath = value;
				GoogleTableSettings.Save();
			}
		}
		
		[TabGroup(ConfigTab), PropertyOrder(TableOrder), Button("UPDATE ALL TABLE", ButtonSizes.Large)]
		private async void UpdateAllTable()
		{
			using var container = new DataContainer();
			await container.Setup();
				
			using var firebase = new FirebaseBootstrap();
			await firebase.Setup();

			var storage = await GetStorageFiles();
			var config = await GetTableConfig();
			DataContainer.Override(config.Model);
			
			try
			{
				EditorUtility.DisplayProgressBar("Table Update", "Updating Table...", 0);

				foreach (var file in storage.Model.Table)
					await FirebaseBootstrap.Storage.GetReference(StoragePath.TableRequest(file)).DeleteAsync();
				
				var sheetRaw = await GoogleTableGenerator.GetSheetAsync();
				await GoogleTableGenerator.GenerateCSharpAsync(sheetRaw);

				var keys = sheetRaw.Keys.ToArray();
				var values = sheetRaw.Values.ToArray();
				for (var i = 0; i < sheetRaw.Count; i++)
				{
					EditorUtility.DisplayProgressBar("Table Update", $"Updating {keys[i]} Table...", (i + 1) / (float)sheetRaw.Count);
					await GoogleTableGenerator.GenerateItemCSharpAsync(keys[i], values[i]);
					
					var storageReference = FirebaseBootstrap.Storage.GetReference(StoragePath.TableRequest(keys[i]));
					var tsv = $"{string.Join("\r\n", values[i])}";
					var metadata = new MetadataChange
					{
						CacheControl = "no-store",
					};
				
					await storageReference.PutBytesAsync(Encoding.UTF8.GetBytes(tsv), metadata);
					Log.Notice($"{keys[i]} Table update is complete.");
				}

				storage.Model.Table = keys;
				await storage.Document.SetAsync(storage.Model);
			}
			catch (Exception e)
			{
				Log.Fail("Error", e.Message);
				EditorUtility.ClearProgressBar();
			}
			
			Log.Success("Table", "Success to update to the Google sheets.");
			EditorUtility.ClearProgressBar();
			
			AssetDatabase.Refresh();
		}
		
		[TabGroup(ConfigTab), PropertyOrder(VersionOrder), Title(VersionTitle), Button("Android")]
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
		
		[TabGroup(ConfigTab), PropertyOrder(VersionOrder), Button("iOS")]
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

		private static async Task<(DocumentReference Document, AppConfigModel Model)> GetAppConfig()
		{
			var document = FirebaseBootstrap.Firestore.Collection(FirebaseDefine.Config).Document(FirebaseDefine.AppConfig);
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
		
		private static async Task<(DocumentReference Document, TableConfigModel Model)> GetTableConfig()
		{
			var document = FirebaseBootstrap.Firestore.Collection(FirebaseDefine.Config).Document(FirebaseDefine.TableConfig);
			var snapshotAsync = await document.GetSnapshotAsync();
			if (snapshotAsync.Exists)
				Log.Success("Firebase", "Success to load to the table config.");
			else
			{
				Log.Fail("Firebase", "Failed to load to the table config.");
				return default;
			}
				
			return (document, snapshotAsync.ConvertTo<TableConfigModel>());
		}
		
		private static async Task<(DocumentReference Document, StorageFileModel Model)> GetStorageFiles()
		{
			var document = FirebaseBootstrap.Firestore.Collection(FirebaseDefine.Storage).Document(ApplicationSettings.Version);
			var snapshotAsync = await document.GetSnapshotAsync();
			if (snapshotAsync.Exists)
			{
				Log.Success("Firebase", "Success to load to the table config.");
				return (document, snapshotAsync.ConvertTo<StorageFileModel>());
			}
			
			var model = new StorageFileModel();
			await document.SetAsync(model);
				
			Log.Fail("Firebase", "Failed to load to the table config.");
			return (document, model);
		}
		
		[Serializable]
		internal class Toggle
		{
			public bool Enabled;
		}
	}
}