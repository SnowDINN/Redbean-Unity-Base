using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Redbean.Editor
{
	internal partial class ApplicationWindow : OdinEditorWindow
	{
		private const string ConfigTab = "Config";
		private const string PlayerPrefsTab = "PlayerPrefs";
		
		private GoogleTableInstaller googleTable;
		
		protected override void OnEnable()
		{
			googleTable = Resources.Load<GoogleTableInstaller>("GoogleTable/GoogleTable");
			
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
	}
}