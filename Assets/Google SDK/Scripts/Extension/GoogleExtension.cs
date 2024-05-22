using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Google
{
	public static class GoogleExtension
	{
		private static Installer m_installer;
		private static Installer installer => m_installer ??= Resources.Load<Installer>("Google/Installer");

		public static string GetAndroidClientId() => installer.GetClientId(ClientType.AndroidClientId);
		public static string GetIosClientId() => installer.GetClientId(ClientType.IosClientId);
		public static string GetIosClientScheme() => installer.GetReverseClientId(ClientType.IosClientId);
		public static string GetWebClientId() => installer.GetClientId(ClientType.WebClientId);
		public static string GetWebSecretId() => installer.webSecretId;
		public static string GetWebRedirectURL() => installer.webRedirectUrl;
		public static int GetWebRedirectUrlPort() => installer.webRedirectPort;

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