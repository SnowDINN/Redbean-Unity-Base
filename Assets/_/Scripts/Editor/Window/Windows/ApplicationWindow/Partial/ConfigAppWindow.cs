using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Redbean.Api;
using Redbean.Bundle;
using Redbean.Table;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace Redbean.Editor
{
	public partial class ApplicationWindow
	{
		private const string ConfigTab = "Config";
		
		private const string ApiGroup = "Tabs/Config/Api";
		private const string BundleGroup = "Tabs/Config/Bundle";
		private const string TableGroup = "Tabs/Config/Table";
		private const string MaintenanceGroup = "Tabs/Config/Maintenance";
		private const string VersionGroup = "Tabs/Config/Version";

		private const int ApiOrder = 100;
		private const int BundleOrder = 200;
		private const int TableOrder = 300;
		private const int MaintenanceOrder = 400;
		private const int VersionOrder = 500;
		
		[TabGroup(TabGroup, ConfigTab), TitleGroup(ApiGroup), PropertyOrder(ApiOrder), LabelText("Create Api Script Path"), ShowInInspector, FolderPath]
		private string ApiGetPath
		{
			get => ApiSettings.ProtocolPath;
			set => ApiSettings.ProtocolPath = value;
		}

		[TabGroup(TabGroup, ConfigTab), TitleGroup(ApiGroup), PropertyOrder(ApiOrder), Button("UPDATE API", ButtonSizes.Large), PropertySpace]
		private async void UpdateApi()
		{
			await ApiGenerator.GetApiAsync(typeof(ApiResponse));
			
			AssetDatabase.Refresh();
		}
		
		[TabGroup(TabGroup, ConfigTab), TitleGroup(BundleGroup), PropertyOrder(BundleOrder), Button("UPDATE BUNDLE", ButtonSizes.Large)]
		private async void UpdateBundle()
		{
			var result = BundleGenerator.TryBuildBundle();
			var buildPath = AddressableAssetSettingsDefaultObject.Settings.RemoteCatalogBuildPath.GetValue(AddressableAssetSettingsDefaultObject.Settings);
			var buildFiles = result.FileRegistry.GetFilePaths().Where(_ => _.Contains(buildPath)).ToArray();
			
			try
			{
				EditorUtility.DisplayProgressBar("Bundle Update", "Updating Bundle...", 0);

				var requestFiles = new List<RequestFile>();
				var path = buildFiles.Select(_ => _.Replace("\\", "/")).ToArray();
				for (var i = 0; i < path.Length; i++)
				{
					var filename = path[i].Split('/').Last();
					EditorUtility.DisplayProgressBar("Bundle Update", $"Updating {filename} Bundle...", (i + 1) / (float)path.Length);
					
					var bytes = await File.ReadAllBytesAsync($"{Application.dataPath.Replace("Assets", "")}{path[i]}");
					requestFiles.Add(new RequestFile
					{
						FileName = filename,
						FileData = bytes
					});
				}
				
				await (await ApiAuthentication.EditorRequestApi<EditBundleFilesProtocol>())
					.Parameter(requestFiles.ToArray())
					.RequestAsync();

				AddressableSettings.Labels = AddressableAssetSettingsDefaultObject.Settings.GetLabels().ToArray();
			}
			catch (Exception e)
			{
				Log.Fail("ERROR", e.Message);
				EditorUtility.ClearProgressBar();
			}
			
			Log.Success("TABLE", "Success to update to the Bundles.");
			EditorUtility.ClearProgressBar();
		}

		[TabGroup(TabGroup, ConfigTab), TitleGroup(TableGroup), PropertyOrder(TableOrder), LabelText("Create Script Path"), ShowInInspector, FolderPath]
		private string TablePath
		{
			get => GoogleTableSettings.Path;
			set => GoogleTableSettings.Path = value;
		}

		[TabGroup(TabGroup, ConfigTab), TitleGroup(TableGroup), PropertyOrder(TableOrder), LabelText("Create Table Script Path"), ShowInInspector, FolderPath]
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
				
				await (await ApiAuthentication.EditorRequestApi<EditTableAccessKeyProtocol>())
					.Parameter()
					.RequestAsync();
				
				var sheetRaw = await GoogleSheetGenerator.GetSheetAsync();
				await GoogleSheetGenerator.GenerateCSharpTableAsync(sheetRaw);
			
				var requestFiles = new List<RequestFile>();
				var keys = sheetRaw.Keys.ToArray();
				var values = sheetRaw.Values.ToArray();
				for (var i = 0; i < sheetRaw.Count; i++)
				{
					EditorUtility.DisplayProgressBar("Table Update", $"Updating {keys[i]} Table...", (i + 1) / (float)sheetRaw.Count);
					await GoogleSheetGenerator.GenerateCSharpSheetAsync(keys[i], values[i]);

					var bytes = Encoding.UTF8.GetBytes($"{string.Join("\r\n", values[i])}");
					requestFiles.Add(new RequestFile
					{
						FileName = $"{keys[i]}.tsv",
						FileData = bytes
					});
				}
				
				await (await ApiAuthentication.EditorRequestApi<EditTableFileProtocol>())
					.Parameter(requestFiles.ToArray())
					.RequestAsync();
			}
			catch (Exception e)
			{
				Log.Fail("ERROR", e.Message);
				EditorUtility.ClearProgressBar();
			}
			
			Log.Success("TABLE", "Success to update to the Google sheets.");
			EditorUtility.ClearProgressBar();
			
			AssetDatabase.Refresh();
		}

		[TabGroup(TabGroup, ConfigTab), TitleGroup(MaintenanceGroup), PropertyOrder(MaintenanceOrder), Button("Maintenance")]
		private async void Maintenance([MultiLineProperty(5)] string contents, DateTime startTime, DateTime endTime)
		{
			await (await ApiAuthentication.EditorRequestApi<EditAppMaintenanceProtocol>())
				.Parameter(contents, startTime, endTime)
				.RequestAsync();
			
			Log.Notice($"Set maintenance update [ Start : {startTime} | End : {endTime} ]");
		}
		
		[TabGroup(TabGroup, ConfigTab), TitleGroup(VersionGroup), PropertyOrder(VersionOrder), Button("Android")]
		private async void AndroidVersion(string version = "0.0.1")
		{
			await (await ApiAuthentication.EditorRequestApi<EditAppVersionProtocol>())
				.Parameter(MobileType.Android, version)
				.RequestAsync();
			
			Log.Notice($"Android version change : {version}");
		}
		
		[TabGroup(TabGroup, ConfigTab), TitleGroup(VersionGroup), PropertyOrder(VersionOrder), Button("iOS"), PropertySpace]
		private async void IosVersion(string version = "0.0.1")
		{
			await (await ApiAuthentication.EditorRequestApi<EditAppVersionProtocol>())
				.Parameter(MobileType.iOS, version)
				.RequestAsync();
			
			Log.Notice($"iOS version changed : {version}");
		}
		
		[Serializable]
		internal class Toggle
		{
			public bool Enabled;
		}
	}
}