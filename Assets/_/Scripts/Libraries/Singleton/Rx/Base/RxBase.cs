using R3;

namespace Redbean.Base
{
	public class RxBase : ISingleton
	{
		protected readonly CompositeDisposable disposables = new();

		public virtual void Dispose()
		{
			disposables?.Dispose();
			disposables?.Clear();
		}
	}
}