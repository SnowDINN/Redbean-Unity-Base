using R3;
using Redbean.Rx;

namespace Redbean.MVP.Content
{
	public class LocalizationPresenter : Presenter
	{
		[View]
		private LocalizationView view;
		
		public override void Setup()
		{
			RxLocalizationBinder.OnLanguageChanged.Subscribe(_ =>
			{
				view.Text.text = this.GetLocalization(view.Localization);
			}).AddTo(this);
			
			view.Text.text = this.GetLocalization(view.Localization);
		}
	}
}