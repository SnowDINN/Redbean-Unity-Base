using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEditor.Build;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.iOS.Xcode;
#endif

namespace Google
{
	public enum ClientType
	{
		AndroidClientId = 1,
		IosClientId,
		WebClientId,
	}
	
	[CreateAssetMenu(fileName = "GoogleSdk", menuName = "Redbean/GoogleSdk")]
	public class GoogleSdkInstaller : ScriptableObject
	{
		[Header("Web")]
		public string webClientId;
		public string webSecretId;
		public int webRedirectPort;
		
		[Header("Android")]
		public string androidClientId;
		
		[Header("iOS")]
		public string iosClientId;
		
		public string webRedirectUrl => $"http://localhost:{webRedirectPort}/";
		
		public string GetClientId(ClientType type)
		{
			var id = string.Empty;

			switch (type)
			{
				case ClientType.AndroidClientId:
				{
#if UNITY_EDITOR
					var jObject = GetJson();
					if (jObject == null)
						id = "google-services.json file does not exist.";
					else
					{
						var packageName = PlayerSettings.GetApplicationIdentifier(NamedBuildTarget.Android);
						var client = jObject["client"]?.Children()
						                              .Where(_ => _.SelectToken("client_info.android_client_info.package_name").Value<string>() == packageName)
						                              .Select(_ => _.SelectToken("oauth_client")).Children()
						                              .FirstOrDefault(_ => _["client_type"].Value<int>() == (int)type)
						                              ?.ToObject<Dictionary<string, object>>();

						if (client != null && client.TryGetValue("client_id", out var clientId))
							id = $"{clientId}";
						else
							id = "Android Client ID value does not exist that matches the package name.";	
					}
#else
						id = androidClientId;
#endif
					
					break;
				}
				
				case ClientType.IosClientId:
				{
#if UNITY_EDITOR
					var plist = GetPlist();
					if (plist == null)
						id = "GoogleService-Info.plist file does not exist.";
					else
					{
						var packageName = PlayerSettings.GetApplicationIdentifier(NamedBuildTarget.iOS);
						id = plist.root["BUNDLE_ID"].AsString() == packageName
							? plist.root["CLIENT_ID"].AsString()
							: "iOS Client ID value does not exist that matches the package name.";
					}
#else
						id = iosClientId;
#endif

					break;
				}
					
				case ClientType.WebClientId:
				{
#if UNITY_EDITOR
					var jObject = GetJson();
					if (jObject == null)
						id = "google-services.json file does not exist.";
					else
					{
						var packageName = PlayerSettings.GetApplicationIdentifier(NamedBuildTarget.Android);
						var client = jObject["client"]?.Children()
						                              .Where(_ => _.SelectToken("client_info.android_client_info.package_name").Value<string>() == packageName)
						                              .Select(_ => _.SelectToken("oauth_client")).Children()
						                              .FirstOrDefault(_ => _["client_type"].Value<int>() == (int)type)
						                              ?.ToObject<Dictionary<string, object>>();

						if (client != null && client.TryGetValue("client_id", out var clientId))
							id = $"{clientId}";
						else
							id = "Web Client ID value does not exist that matches the package name.";		
					}
#else
						id = webClientId;
#endif
					
					break;
				}
			}

			return id;
		}

		public string GetReverseClientId(ClientType type)
		{
			var split = GetClientId(type).Split('.').Reverse();
			return string.Join('.', split);
		}

#if UNITY_EDITOR
		public JObject GetJson()
		{
			var text = string.Empty;
			
			var guids = AssetDatabase.FindAssets("google-services");
			if (guids.Length == 0)
				return null;
			
			foreach (var guid in guids)
			{
				var path = AssetDatabase.GUIDToAssetPath(guid);
				if (!path.Contains("google-services.json"))
					continue;

				text = File.ReadAllText(path);
				break;
			}
			
			if (string.IsNullOrEmpty(text))
				return null;

			return JObject.Parse(text);
		}

		public PlistDocument GetPlist()
		{
			var text = string.Empty;
			
			var guids = AssetDatabase.FindAssets("GoogleService-Info");
			if (guids.Length == 0)
				return null;
			
			foreach (var guid in guids)
			{
				var path = AssetDatabase.GUIDToAssetPath(guid);
				if (!path.Contains("GoogleService-Info.plist"))
					continue;

				text = File.ReadAllText(path);
				break;
			}

			if (string.IsNullOrEmpty(text))
				return null;

			var plist = new PlistDocument();
			plist.ReadFromString(text);
			
			return plist;
		}

		public void Save()
		{
			EditorUtility.SetDirty(this);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
#endif
	}
}