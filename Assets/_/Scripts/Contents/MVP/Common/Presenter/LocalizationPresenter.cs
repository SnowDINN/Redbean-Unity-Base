using Redbean.Table;

namespace Redbean.MVP.Content
{
	public class LocalizationPresenter : Presenter
	{
		[View]
		private LocalizationView view;
		
		public override void Setup()
		{
			view.Text.text = TableContainer.Localization[view.Localization].Kr;
		}
	}
}