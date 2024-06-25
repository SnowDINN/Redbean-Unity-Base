using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Redbean.Api;
using Redbean.Singleton;
using UnityEngine;

namespace Redbean.Editor
{
	internal partial class ApplicationWindow : WindowBase
	{
		private const string TabGroup = "Tabs";
		
		protected override void OnEnable()
		{
			if (!PlayerPrefs.HasKey(MvpSingletonContainer.PLAYER_PREFS_KEY))
				return;

			var dataDecrypt = PlayerPrefs.GetString(MvpSingletonContainer.PLAYER_PREFS_KEY).Decryption();
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