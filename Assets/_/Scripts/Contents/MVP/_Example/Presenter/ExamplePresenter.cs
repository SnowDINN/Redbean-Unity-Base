using R3;
using Redbean.Popup.Content;

namespace Redbean.MVP.Content
{
	public class ExamplePresenter : Presenter
	{
		[View]
		private ButtonView view;
		
		public override void Setup()
		{
			view.Button.AsButtonObservable().Subscribe(_ =>
			{
				this.Popup().Open<PopupExample>();
			}).AddTo(this);
		}
	}
}