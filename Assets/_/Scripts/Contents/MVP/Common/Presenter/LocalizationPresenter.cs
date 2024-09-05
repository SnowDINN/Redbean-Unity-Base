using Redbean.Table;

namespace Redbean.MVP.Content
{
	public class LocalizationPresenter : Presenter
	{
		[View]
		private LocalizationView view;
		
		public override void Setup()
		{
			if (TableContainer.Localization.TryGetValue(view.Localization, out var value))
			{
				view.Text.text = value.Kr;
				return;
			}

			view.Text.text = view.Localization;
		}
	}
}