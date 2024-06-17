using Google;
using UnityEngine;

namespace Redbean.Editor
{
	internal partial class ThirdPartyWindow : WindowBase
	{
		private const string TabGroup = "Tabs";
		
		private GoogleSdkInstaller googleSdk;
		
		protected override void OnEnable()
		{
			googleSdk = Resources.Load<GoogleSdkInstaller>("Google/GoogleSdk");
		}
	}
}