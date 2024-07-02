using TMPro;
using UnityEngine;

namespace Redbean.Popup.Content
{
	public class PopupException : PopupBase
	{
		[SerializeField] 
		private TextMeshProUGUI text;
		
		public override void Awake()
		{
			base.Awake();
			
			text.text = AppSettings.ExceptionMessage;
		}
	}
}