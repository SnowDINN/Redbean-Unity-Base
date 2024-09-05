using System;
using Redbean.MVP.Content;
using TMPro;
using UnityEngine;

namespace Redbean.Popup.Content
{
	public class PopupMaintenance : PopupBase
	{
		[SerializeField]
		private TextMeshProUGUI text;

		public override void Awake()
		{
			base.Awake();

			var maintenance = this.GetModel<AppSettingModel>().Database.Maintenance;
			text.text =
				$"Server Maintenance\n\n{DateTime.Parse(maintenance.Time.StartTime):hh:mm} ~ {DateTime.Parse(maintenance.Time.EndTime):hh:mm}\n\n{maintenance.Contents}";
		}
	}
}