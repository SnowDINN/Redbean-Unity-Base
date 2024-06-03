using Google;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Redbean.Editor
{
	internal partial class ThirdPartyWindow : OdinEditorWindow
	{
		private const string TabGroup = "Tabs";
		
		private GoogleSdkInstaller googleSdk;
		
		protected override void OnEnable()
		{
			googleSdk = Resources.Load<GoogleSdkInstaller>("Google/GoogleSdk");
		}
	}
}