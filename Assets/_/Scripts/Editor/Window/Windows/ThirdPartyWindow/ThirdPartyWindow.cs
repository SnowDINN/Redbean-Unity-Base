using Google;
using UnityEngine;

namespace Redbean.Editor
{
	internal partial class ThirdPartyWindow : WindowBase
	{
		private const string TabGroup = "Tabs";
		
		private GoogleAuthenticationInstaller googleAuth;
		
		protected override void OnEnable()
		{
			googleAuth = Resources.Load<GoogleAuthenticationInstaller>("Google/GoogleAuthentication");
		}
	}
}