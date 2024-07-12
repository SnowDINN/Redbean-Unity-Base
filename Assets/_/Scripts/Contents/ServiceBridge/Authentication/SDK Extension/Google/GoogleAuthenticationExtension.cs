using UnityEngine;

namespace Google
{
	public static class GoogleAuthenticationExtension
	{
		private static GoogleAuthenticationInstaller googleAuthInstaller;
		private static GoogleAuthenticationInstaller GoogleAuthInstaller => googleAuthInstaller ??= Resources.Load<GoogleAuthenticationInstaller>("Google/GoogleAuthentication");

		public static string GetAndroidClientId() => GoogleAuthInstaller.GetClientId(ClientType.AndroidClientId);
		public static string GetIosClientId() => GoogleAuthInstaller.GetClientId(ClientType.IosClientId);
		public static string GetIosClientScheme() => GoogleAuthInstaller.GetReverseClientId(ClientType.IosClientId);
		public static string GetWebClientId() => GoogleAuthInstaller.GetClientId(ClientType.WebClientId);
		public static string GetWebSecretId() => GoogleAuthInstaller.webClientSecretId;
	}   
}