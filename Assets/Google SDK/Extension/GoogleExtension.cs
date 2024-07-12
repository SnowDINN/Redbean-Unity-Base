using UnityEngine;

namespace Google
{
	public static class GoogleExtension
	{
		private static GoogleSdkInstaller googleSdkInstaller;
		private static GoogleSdkInstaller GoogleSdkInstaller => googleSdkInstaller ??= Resources.Load<GoogleSdkInstaller>("Google/GoogleSdk");

		public static string GetAndroidClientId() => GoogleSdkInstaller.GetClientId(ClientType.AndroidClientId);
		public static string GetIosClientId() => GoogleSdkInstaller.GetClientId(ClientType.IosClientId);
		public static string GetIosClientScheme() => GoogleSdkInstaller.GetReverseClientId(ClientType.IosClientId);
		public static string GetWebClientId() => GoogleSdkInstaller.GetClientId(ClientType.WebClientId);
		public static string GetWebSecretId() => GoogleSdkInstaller.webClientSecretId;
	}   
}