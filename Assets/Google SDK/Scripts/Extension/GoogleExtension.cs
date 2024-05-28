using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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
		public static string GetWebSecretId() => GoogleSdkInstaller.webSecretId;
		public static string GetWebRedirectURL() => GoogleSdkInstaller.webRedirectUrl;
		public static int GetWebRedirectUrlPort() => GoogleSdkInstaller.webRedirectPort;

		public static async UniTask<(GoogleSignInUser user, int code)> AwaitCompleted(this Task<GoogleSignInUser> task)
		{
			await UniTask.WaitUntil(() => task.IsCompleted);
			
			if (task.IsCanceled || task.IsFaulted)
			{
				using var enumerator = task.Exception.InnerExceptions.GetEnumerator();
				if (enumerator.MoveNext())
				{
					var error = (GoogleSignIn.SignInException)enumerator.Current;
					if (error != null)
						return (task.Result, (int)error.Status);
				}
			}
			else
				return (task.Result, (int)GoogleSignInStatusCode.SUCCESS);
			
			return (task.Result, int.MaxValue);
		}
	}   
}