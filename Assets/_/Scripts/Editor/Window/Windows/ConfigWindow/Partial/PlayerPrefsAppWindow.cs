using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Redbean.Editor
{
	internal partial class ApplicationWindow
	{
		private const string PlayerPrefsTab = "PlayerPrefs";
		private const string PlayerPrefsGroup = "Tabs/PlayerPrefs/PlayerPrefs Information";
		private const int PlayerPrefsOrder = 10;

		[TabGroup(TabGroup, PlayerPrefsTab), TitleGroup(PlayerPrefsGroup), PropertyOrder(PlayerPrefsOrder), ShowInInspector, ReadOnly]
		private Dictionary<string, object> playerPrefsGroup = new();
	}
}