using System.Collections.Generic;
using Redbean.Cryptography;
using Sirenix.OdinInspector;

namespace Redbean.Editor
{
	internal partial class ApplicationWindow
	{
		private readonly AES128 aes = new();

		[TabGroup("Tabs", PlayerPrefsTab), Title(""), ShowInInspector, ReadOnly]
		private Dictionary<string, object> playerPrefsGroup = new();
	}
}