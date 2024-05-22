using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Firestore;
using Redbean.Firebase;
using Redbean.MVP.Content;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Redbean.Editor
{
	internal class ConfigWindow : OdinEditorWindow
	{
		private const string Version = nameof(Version);
		
		[TitleGroup(Version), Button]
		public async void AndroidVersion(string version = "0.0.1")
		{
			var config = await GetAppConfig();
			var before = config.Model.Android.Version;
			config.Model.Android.Version = version;

			await config.Document.SetAsync(config.Model);
			
			FirebaseApp.DefaultInstance.Dispose();
			
			Log.Notice($"Android version changed from {before} -> {version}.");
		}
		
		[TitleGroup(Version), Button]
		public async void IosVersion(string version = "0.0.1")
		{
			var config = await GetAppConfig();
			var before = config.Model.iOS.Version;
			config.Model.iOS.Version = version;

			await config.Document.SetAsync(config.Model);
			
			FirebaseApp.DefaultInstance.Dispose();
			
			Log.Notice($"Android version changed from {before} -> {version}.");
		}

		private async UniTask<(DocumentReference Document, AppConfigModel Model)> GetAppConfig()
		{
			await new FirebaseCore().Setup();

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