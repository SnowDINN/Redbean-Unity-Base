using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase;
using Firebase.Firestore;
using Firebase.Storage;
using Redbean.Api;
using Redbean.Bundle;
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
		private const string ConfigTab = "Config";
		
		private const string ApiGroup = "Tabs/Config/Api";
		private const string BundleGroup = "Tabs/Config/Bundle";
		private const string TableGroup = "Tabs/Config/Table";
		private const string VersionGroup = "Tabs/Config/Version";

		private const int ApiOrder = 100;
		private const int BundleOrder = 200;
		private const int TableOrder = 300;
		private const int VersionOrder = 400;
		
		[TabGroup(TabGroup, ConfigTab), TitleGroup(ApiGroup), PropertyOrder(ApiOrder), LabelText("Get Path"), ShowInInspector, FolderPath]
		private string ApiGetPath
		{
			get => ApiSettings.ProtocolPath;
			set => ApiSettings.ProtocolPath = value;
		}

		[TabGroup(TabGroup, ConfigTab), TitleGroup(ApiGroup), PropertyOrder(ApiOrder), Button("UPDATE API", ButtonSizes.Large), PropertySpace]
		private async void UpdateApi()
		{
			await ApiGenerator.GetApiAsync();
			
			AssetDatabase.Refresh();
		}
		
		[TabGroup(TabGroup, ConfigTab), TitleGroup(BundleGroup), PropertyOrder(BundleOrder), Button("UPDATE BUNDLE", ButtonSizes.Large)]
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

#if UNITY_ANDROID
				// 기존 번들 제거
				await ApiDeleteRequest.DeleteAndroidBundleFilesRequest(ApplicationSettings.Version);
#endif
				
#if UNITY_IOS
				// 기존 번들 제거
				await ApiDeleteRequest.DeleteiOSBundleFilesRequest(ApplicationSettings.Version);
#endif

				var path = buildFiles.Select(_ => _.Replace("\\", "/")).ToArray();
				for (var i = 0; i < path.Length; i++)
				{
					var filename = path[i].Split('/').Last();
					EditorUtility.DisplayProgressBar("Bundle Update", $"Updating {filename} Bundle...", (i + 1) / (float)path.Length);
					var storageReference = Extension.Storage.GetReference(path[i]);
					var metadata = new MetadataChange
					{
						CacheControl = "no-store",
					};
				
					await storageReference.PutFileAsync($"{Application.dataPath.Replace("Assets", "")}{path[i]}", metadata);
					Log.Notice($"{filename} Bundle update is complete.");
				}

				AddressableSettings.Labels = AddressableAssetSettingsDefaultObject.Settings.GetLabels().ToArray();
			}
			catch (Exception e)
			{
				Log.Fail("Error", e.Message);
				EditorUtility.ClearProgressBar();
			}
			
			Log.Success("Table", "Success to update to the Bundles.");
			EditorUtility.ClearProgressBar();
		}

		[TabGroup(TabGroup, ConfigTab), TitleGroup(TableGroup), PropertyOrder(TableOrder), LabelText("Path"), ShowInInspector, FolderPath]
		private string TablePath
		{
			get => GoogleTableSettings.Path;
			set => GoogleTableSettings.Path = value;
		}

		[TabGroup(TabGroup, ConfigTab), TitleGroup(TableGroup), PropertyOrder(TableOrder), LabelText("Item Path"), ShowInInspector, FolderPath]
		private string TableItemPath
		{
			get => GoogleTableSettings.ItemPath;
			set => GoogleTableSettings.ItemPath = value;
		}

		[TabGroup(TabGroup, ConfigTab), TitleGroup(TableGroup), HorizontalGroup("Tabs/Config/Table/Horizontal"), PropertyOrder(TableOrder), Button("OPEN TABLE", ButtonSizes.Large), PropertySpace]
		private void OpenTableURL()
		{
			Application.OpenURL("https://docs.google.com/spreadsheets/d/1UjQhF5Zhxpa-2bP5hoIH2kVClHT5ZM_QBbrAiE3c3xs/edit#gid=924887524");
		}
		
		[TabGroup(TabGroup, ConfigTab), TitleGroup(TableGroup), HorizontalGroup("Tabs/Config/Table/Horizontal"), PropertyOrder(TableOrder), Button("UPDATE ALL TABLE", ButtonSizes.Large), PropertySpace]
		private async void UpdateAllTable()
		{
			using var firebase = new FirebaseBootstrap();
			await firebase.Setup();

			try
			{
				EditorUtility.DisplayProgressBar("Table Update", "Updating Table...", 0);
			
				// 기존 테이블 제거
				await ApiDeleteRequest.DeleteTableFilesRequest(ApplicationSettings.Version);
				
				var sheetRaw = await GoogleTableGenerator.GetSheetAsync();
				await GoogleTableGenerator.GenerateCSharpAsync(sheetRaw);
			
				var keys = sheetRaw.Keys.ToArray();
				var values = sheetRaw.Values.ToArray();
				for (var i = 0; i < sheetRaw.Count; i++)
				{
					EditorUtility.DisplayProgressBar("Table Update", $"Updating {keys[i]} Table...", (i + 1) / (float)sheetRaw.Count);
					await GoogleTableGenerator.GenerateItemCSharpAsync(keys[i], values[i]);
					
					var storageReference = Extension.Storage.GetReference(StoragePath.TableRequest(keys[i]));
					var tsv = $"{string.Join("\r\n", values[i])}";
					var metadata = new MetadataChange
					{
						CacheControl = "no-store",
					};
				
					await storageReference.PutBytesAsync(Encoding.UTF8.GetBytes(tsv), metadata);
					Log.Notice($"{keys[i]} Table update is complete.");
				}
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
		
		[TabGroup(TabGroup, ConfigTab), TitleGroup(VersionGroup), PropertyOrder(VersionOrder), Button("Android")]
		private async void AndroidVersion(string version = "0.0.1")
		{
			var request = await ApiPostRequest.PostAndroidVersionRequest(version);
			var response = request.ToConvert<AppVersionResponse>();
			
			Log.Notice($"Android version changed from {response.BeforeVersion} -> {response.AfterVersion}.");
		}
		
		[TabGroup(TabGroup, ConfigTab), TitleGroup(VersionGroup), PropertyOrder(VersionOrder), Button("iOS")]
		private async void IosVersion(string version = "0.0.1")
		{
			var request = await ApiPostRequest.PostiOSVersionRequest(version);
			var response = request.ToConvert<AppVersionResponse>();
			
			Log.Notice($"iOS version changed from {response.BeforeVersion} -> {response.AfterVersion}.");
		}
		
		[Serializable]
		internal class Toggle
		{
			public bool Enabled;
		}
	}
}