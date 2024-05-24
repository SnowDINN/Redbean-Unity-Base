using R3;
using Redbean.Popup.Content;

namespace Redbean.MVP.Content
{
	public class PopupExceptionPresenter : Presenter
	{
		[View]
		private TextView view;
		
		public override void Setup()
		{
			var popup = view.GetComponent<PopupException>();
			popup.ExceptionMessage.Subscribe(_ =>
			{
				view.Text.text = _;
			}).AddTo(this);
		}
	}
}