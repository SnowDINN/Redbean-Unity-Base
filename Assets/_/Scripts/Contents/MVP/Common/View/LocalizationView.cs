using TMPro;
using UnityEngine;

namespace Redbean.MVP.Content
{
	public class LocalizationView : View
	{
		[SerializeField, TextArea] private string localization;
		public string Localization => localization;
		
		[SerializeField] private TextMeshProUGUI text;
		public TextMeshProUGUI Text => text;
	}
}