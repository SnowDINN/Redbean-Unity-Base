using R3;
using Redbean.MVP;

namespace Redbean.Rx
{
	public class RxModelBinder : RxBase, ISingletonContainer
	{
		private readonly Subject<IModel> onModelChanged = new();
		public Observable<IModel> OnModelChanged => onModelChanged.Share();
		
		public RxModelBinder()
		{
			OnModelChanged.Where(_ => _ is ISerializeModel)
				.Select(_ => _.As<ISerializeModel>())
				.Subscribe(_ =>
				{
					_.Rx.Publish(_);
				}).AddTo(disposables);
		}

		public T Publish<T>(T value) where T : IModel
		{
			onModelChanged.OnNext(value);
			return value;
		}
	}
}