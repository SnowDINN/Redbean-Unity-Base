using Google;
using Redbean.Table;
using UnityEngine;

namespace Redbean.Editor
{
	internal partial class ThirdPartyWindow : WindowBase
	{
		private const string TabGroup = "Tabs";
		
		private GoogleAuthScriptable googleAuth;
		private GoogleSheetScriptable googleSheet;
		
		protected override void OnEnable()
		{
			googleAuth = Resources.Load<GoogleAuthScriptable>(nameof(GoogleAuthScriptable));
			googleSheet = ApplicationLoader.GetScriptable<GoogleSheetScriptable>();
		}
	}
}