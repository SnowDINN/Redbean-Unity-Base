using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Redbean.Cryptography;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Redbean.Editor
{
	internal partial class ApplicationWindow
	{
		private readonly AES128 aes = new();
		
		protected override void OnEnable()
		{
			if (!PlayerPrefs.HasKey(Key.GetDataGroup))
				return;

			var dataDecrypt = aes.Decrypt(PlayerPrefs.GetString(Key.GetDataGroup));
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

		[TabGroup(PlayerPrefsTab), Title(""), ShowInInspector, ReadOnly]
		private Dictionary<string, object> playerPrefsGroup = new();
	}
}