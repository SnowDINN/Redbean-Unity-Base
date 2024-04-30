using R3;
using Redbean.Base;
using Redbean.MVP;

namespace Redbean.Rx
{
	public class RxModelBinder : RxBase
	{
		private readonly Subject<object> onModelChanged = new();
		public Observable<object> OnModelChanged => onModelChanged.Share();
		
		public RxModelBinder()
		{
			OnModelChanged.Where(_ => _ is ISerializeModel)
			              .Select(_ => (ISerializeModel)_)
			              .Subscribe(_ =>
			              {
				              _.Rx.Publish(_);
			              }).AddTo(disposables);
		}

		public T Publish<T>(T value)
		{
			onModelChanged.OnNext(value);
			return value;
		}
	}
}