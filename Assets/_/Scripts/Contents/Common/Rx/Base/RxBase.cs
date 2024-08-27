using R3;

namespace Redbean.Rx
{
	public class RxBase : IExtension
	{
		protected readonly CompositeDisposable disposables = new();

		public virtual void Setup()
		{
		}

		public virtual void Teardown()
		{
			disposables?.Dispose();
			disposables?.Clear();
		}
	}
}