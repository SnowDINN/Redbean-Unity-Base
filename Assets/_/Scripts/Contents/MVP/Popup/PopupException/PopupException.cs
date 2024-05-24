using R3;

namespace Redbean.Popup.Content
{
	public class PopupException : PopupBase
	{
		public ReactiveProperty<string> ExceptionMessage = new();
	}
}