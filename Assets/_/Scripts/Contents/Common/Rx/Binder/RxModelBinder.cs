using R3;
using Redbean.MVP;

namespace Redbean.Rx
{
	public class RxModelBinder : RxBase
	{
		private static readonly Subject<IModel> onChanged = new();
		public static Observable<IModel> OnChanged => onChanged.Share();

		public static T Publish<T>(T value) where T : IModel
		{
			onChanged.OnNext(value);
			return value;
		}
	}
}