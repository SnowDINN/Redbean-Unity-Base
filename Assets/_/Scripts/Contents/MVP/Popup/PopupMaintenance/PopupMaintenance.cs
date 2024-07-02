using System;
using Redbean.Api;
using Redbean.MVP.Content;
using TMPro;
using UnityEngine;

namespace Redbean.Popup.Content
{
	public class PopupMaintenance : PopupBase
	{
		[SerializeField]
		private TextMeshProUGUI text;

		private AppMaintenanceConfig maintenance => 
			this.GetModel<AppConfigModel>().Maintenance;

		public override void Awake()
		{
			base.Awake();
			
			text.text =
				$"Server Maintenance\n\n{DateTime.Parse(maintenance.Time.StartTime):hh:mm} ~ {DateTime.Parse(maintenance.Time.EndTime):hh:mm}\n\n{maintenance.Contents}";
		}
	}
}