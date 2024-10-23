using Google;
using UnityEngine;

namespace Redbean.Editor
{
	internal partial class ThirdPartyWindow : WindowBase
	{
		private const string TabGroup = "Tabs";
		
		private GoogleAuthScriptable googleAuth;
		
		protected override void OnEnable()
		{
			googleAuth = Resources.Load<GoogleAuthScriptable>("Google/GoogleAuthentication");
		}
	}
}