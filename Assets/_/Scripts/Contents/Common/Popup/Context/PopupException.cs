using TMPro;
using UnityEngine;

namespace Redbean.Popup.Content
{
	public class PopupException : PopupBase
	{
		[SerializeField] 
		private TextMeshProUGUI text;

		public string ExceptionMessage
		{
			set => text.text = value;
		}
	}
}