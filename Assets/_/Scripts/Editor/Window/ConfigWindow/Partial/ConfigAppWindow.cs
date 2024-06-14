using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Redbean.Api;
using Redbean.Bundle;
using Redbean.Firebase;
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
			using var firebase = new AppSequenceBootstrap();
			await firebase.Setup();
			
			var result = BundleGenerator.TryBuildBundle();
			var buildPath = AddressableAssetSettingsDefaultObject.Settings.RemoteCatalogBuildPath.GetValue(AddressableAssetSettingsDefaultObject.Settings);
			var buildFiles = result.FileRegistry.GetFilePaths().Where(_ => _.Contains(buildPath)).ToArray();

			EditorUtility.DisplayProgressBar("Bundle Update", "Updating Bundle...", 0);

			var content = new MultipartFormDataContent();

			var path = buildFiles.Select(_ => _.Replace("\\", "/")).ToArray();
			for (var i = 0; i < path.Length; i++)
			{
				var filename = path[i].Split('/').Last();
				EditorUtility.DisplayProgressBar("Bundle Update", $"Updating {filename} Bundle...", (i + 1) / (float)path.Length);
					
				var bytes = await File.ReadAllBytesAsync($"{Application.dataPath.Replace("Assets", "")}{path[i]}");
				content.Add(new ByteArrayContent(bytes), "bundles", filename);
			}
				
			await ApiSingleton.EditorRequestApi<PostBundleFilesProtocol>(content);

			AddressableSettings.Labels = AddressableAssetSettingsDefaultObject.Settings.GetLabels().ToArray();
			
			try
			{

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
			try
			{
				EditorUtility.DisplayProgressBar("Table Update", "Updating Table...", 0);

				var content = new MultipartFormDataContent();
				
				var sheetRaw = await GoogleTableGenerator.GetSheetAsync();
				await GoogleTableGenerator.GenerateCSharpAsync(sheetRaw);
			
				var keys = sheetRaw.Keys.ToArray();
				var values = sheetRaw.Values.ToArray();
				for (var i = 0; i < sheetRaw.Count; i++)
				{
					EditorUtility.DisplayProgressBar("Table Update", $"Updating {keys[i]} Table...", (i + 1) / (float)sheetRaw.Count);
					await GoogleTableGenerator.GenerateItemCSharpAsync(keys[i], values[i]);

					var bytes = Encoding.UTF8.GetBytes($"{string.Join("\r\n", values[i])}");
					content.Add(new ByteArrayContent(bytes), "tables", $"{keys[i]}.tsv");
				}
				
				await ApiSingleton.EditorRequestApi<PostTableFileProtocol>(content);
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
			var request = await ApiSingleton.EditorRequestApi<PostAppVersionProtocol>(version, (int)MobileType.Android);
			var response = request.ToConvert<AppVersionResponse>();
			
			Log.Notice($"Android version changed from {response.BeforeVersion} -> {response.AfterVersion}.");
		}
		
		[TabGroup(TabGroup, ConfigTab), TitleGroup(VersionGroup), PropertyOrder(VersionOrder), Button("iOS")]
		private async void IosVersion(string version = "0.0.1")
		{
			var request = await ApiSingleton.EditorRequestApi<PostAppVersionProtocol>(version, (int)MobileType.iOS);
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