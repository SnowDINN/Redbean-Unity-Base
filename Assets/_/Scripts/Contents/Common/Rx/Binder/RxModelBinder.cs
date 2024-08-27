using R3;
using Redbean.MVP;

namespace Redbean.Rx
{
	public class RxModelBinder : RxBase
	{
		private static readonly Subject<IModel> onModelChanged = new();
		public static Observable<IModel> OnModelChanged => onModelChanged.Share();

		public override void Setup()
		{
			base.Setup();
			
			OnModelChanged.Where(_ => _ is ISerializeModel)
				.Select(_ => _.As<ISerializeModel>())
				.Subscribe(_ =>
				{
					_.Rx.Publish(_);
				}).AddTo(disposables);
		}

		public static T Publish<T>(T value) where T : IModel
		{
			onModelChanged.OnNext(value);
			return value;
		}
	}
}