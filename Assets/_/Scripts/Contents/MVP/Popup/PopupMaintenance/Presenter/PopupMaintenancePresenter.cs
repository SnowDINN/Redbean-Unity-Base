using System;

namespace Redbean.MVP.Content
{
	public class PopupMaintenancePresenter : Presenter
	{
		[View]
		private TextView view;

		[Model]
		private AppConfigModel model;

		public override void Setup()
		{
			var maintenance = model.Maintenance;
			view.Text.text = $"Server Maintenance\n\n{DateTime.Parse(maintenance.Time.StartTime):hh:mm} ~ {DateTime.Parse(maintenance.Time.EndTime):hh:mm}\n\n{maintenance.Contents}";
		}
	}
}