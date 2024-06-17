using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Redbean.Singleton;
using UnityEngine;

namespace Redbean.Editor
{
	internal partial class ApplicationWindow : WindowBase
	{
		private const string TabGroup = "Tabs";
		
		protected override void OnEnable()
		{
			if (!PlayerPrefs.HasKey(MvpSingleton.PLAYER_PREFS_KEY))
				return;

			var dataDecrypt = aes.Decrypt(PlayerPrefs.GetString(MvpSingleton.PLAYER_PREFS_KEY));
			var dataGroups = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataDecrypt);
			if (dataGroups == null)
				return;

			foreach (var dataGroup in dataGroups)
			{
				var key = Assembly.Load("Assembly-CSharp").GetTypes().FirstOrDefault(_ => _.FullName == dataGroup.Key);
				var value = JsonConvert.DeserializeObject(dataGroup.Value, key);

				playerPrefsGroup.Add(key.Name, value);
			}
		}
	}
}