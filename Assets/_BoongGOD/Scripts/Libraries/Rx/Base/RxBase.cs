using R3;
using Redbean.Static;

namespace Redbean.Base
{
	public class RxBase : ISingleton
	{
		protected readonly CompositeDisposable disposables = new();
		
		public virtual void Dispose()
		{
			disposables.Dispose();
			disposables.Clear();
		}
	}
}