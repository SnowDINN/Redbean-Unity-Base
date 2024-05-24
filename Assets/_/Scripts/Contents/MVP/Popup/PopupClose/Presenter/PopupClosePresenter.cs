using R3;
using Redbean.Popup;

namespace Redbean.MVP.Content
{
	public class PopupClosePresenter : Presenter
	{
		[View]
		private ButtonView view;

		public override void Setup()
		{
			view.Button.AsButtonObservable().Subscribe(_ =>
			{
				view.GetComponent<PopupBase>().Close();
			}).AddTo(this);
		}
	}
}