using UnityEngine;

namespace Google
{
	public static class GoogleAuthenticationClient
	{
		private static readonly GoogleAuthenticationInstaller GoogleAuthInstaller = Resources.Load<GoogleAuthenticationInstaller>("Google/GoogleAuthentication");

		public static string GetAndroidClientId() => GoogleAuthInstaller.GetClientId(ClientType.AndroidClientId);
		public static string GetIosClientId() => GoogleAuthInstaller.GetClientId(ClientType.IosClientId);
		public static string GetIosClientScheme() => GoogleAuthInstaller.GetReverseClientId(ClientType.IosClientId);
		public static string GetWebClientId() => GoogleAuthInstaller.GetClientId(ClientType.WebClientId);
	}   
}