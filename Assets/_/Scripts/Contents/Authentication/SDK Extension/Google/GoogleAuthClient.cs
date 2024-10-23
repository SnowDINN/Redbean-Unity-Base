using UnityEngine;

namespace Google
{
	public static class GoogleAuthClient
	{
		private static readonly GoogleAuthScriptable GoogleAuthScriptable = Resources.Load<GoogleAuthScriptable>("GoogleAuthScriptable");

		public static string GetAndroidClientId() => GoogleAuthScriptable.GetClientId(ClientType.AndroidClientId);
		public static string GetIosClientId() => GoogleAuthScriptable.GetClientId(ClientType.IosClientId);
		public static string GetIosClientScheme() => GoogleAuthScriptable.GetReverseClientId(ClientType.IosClientId);
		public static string GetWebClientId() => GoogleAuthScriptable.GetClientId(ClientType.WebClientId);
	}   
}