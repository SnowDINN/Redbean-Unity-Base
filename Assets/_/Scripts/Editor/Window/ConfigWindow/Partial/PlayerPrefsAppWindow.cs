using System.Collections.Generic;
using Newtonsoft.Json;
using Redbean.Cryptography;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Redbean.Editor
{
	internal partial class ApplicationWindow
	{
		private AES128 aes;
		
		protected override void OnEnable()
		{
			aes = new AES128();
			
			if (!PlayerPrefs.HasKey(Key.GetDataGroup))
				return;

			var decryptValue = aes.Decrypt(PlayerPrefs.GetString(Key.GetDataGroup));
				
			var deserializer = JsonConvert.DeserializeObject<Dictionary<string, string>>(decryptValue);
			if (deserializer != null)
				playerPrefsGroup = deserializer;
		}

		[TabGroup(PlayerPrefsTab), ShowInInspector]
		private Dictionary<string, string> playerPrefsGroup = new();
	}
}