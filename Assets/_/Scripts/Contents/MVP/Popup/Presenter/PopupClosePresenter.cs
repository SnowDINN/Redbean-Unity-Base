using R3;
using Redbean.Popup;

namespace Redbean.MVP.Content
{
	public class PopupClosePresenter : Presenter
	{
		[View]
		private ButtonView view;

		private PopupBase popup;

		public override void Setup()
		{
			popup = view.GetComponent<PopupBase>();

			view.Button.AsButtonObservable().Subscribe(_ =>
			{
				popup.Close();
			}).AddTo(this);
		}
	}
}