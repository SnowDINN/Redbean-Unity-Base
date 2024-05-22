using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

#if UNITY_IOS
using System.IO;
using UnityEditor.iOS.Xcode;
#endif

namespace Google
{
	public class GoogleSignInPostBuild : IPostprocessBuildWithReport
	{
		public int callbackOrder => 999;

		public void OnPostprocessBuild(BuildReport report)
		{
			OnPostProcessBuild(report.summary.platform, report.summary.outputPath);
		}

		private void OnPostProcessBuild(BuildTarget target, string path)
		{
			var installer = Resources.Load<Installer>("GoogleSignInInstaller");
			installer.androidClientId = GoogleExtension.GetAndroidClientId();
			installer.iosClientId = GoogleExtension.GetIosClientId();
			installer.webClientId = GoogleExtension.GetWebClientId();
			installer.webSecretId = "";
			installer.Save();

#if UNITY_IOS
			GoogleServiesPlist(path);
			InfoPlist(path);
#endif
		}

#if UNITY_IOS
		private void GoogleServiesPlist(string path)
		{
			var googlePlistPath = $"{path}/GoogleService-Info.plist";
			var googlePlist = new PlistDocument();
			googlePlist.ReadFromString(File.ReadAllText(googlePlistPath));
			googlePlist.root.SetString("CLIENT_ID", GoogleExtension.GetIosClientId());
			googlePlist.root.SetString("REVERSED_CLIENT_ID", GoogleExtension.GetIosClientScheme());
			googlePlist.WriteToFile(googlePlistPath);
		}

		private void InfoPlist(string path)
		{
			var plistPath = $"{path}/Info.plist";
			var plist = new PlistDocument();
			plist.ReadFromString(File.ReadAllText(plistPath));
			
			var urlTypes = plist.root["CFBundleURLTypes"];
			var urlSchemes = urlTypes.AsArray();
			urlSchemes.values.RemoveAll(_ =>
			{
				var name = string.Empty;
				if (_.AsDict().values.TryGetValue("CFBundleURLName", out var value))
					name = value.AsString();

				return name == "google";
			});
			
			var dictionary = urlSchemes.AddDict();
			dictionary.SetString("CFBundleURLName", "google");
			dictionary.CreateArray("CFBundleURLSchemes")
			          .AddString(GoogleExtension.GetIosClientScheme());
			
			plist.WriteToFile(plistPath);
		}
#endif
	}	
}