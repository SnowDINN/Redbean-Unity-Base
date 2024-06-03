using System.Collections.Generic;
using Redbean.Cryptography;
using Sirenix.OdinInspector;

namespace Redbean.Editor
{
	internal partial class ApplicationWindow
	{
		private const string PlayerPrefsTab = "PlayerPrefs";
		private const string PlayerPrefsGroup = "Tabs/PlayerPrefs/PlayerPrefs Information";
		private const int PlayerPrefsOrder = 10;
		
		private readonly AES128 aes = new();

		[TabGroup(TabGroup, PlayerPrefsTab), TitleGroup(PlayerPrefsGroup), PropertyOrder(PlayerPrefsOrder), ShowInInspector, ReadOnly]
		private Dictionary<string, object> playerPrefsGroup = new();
	}
}