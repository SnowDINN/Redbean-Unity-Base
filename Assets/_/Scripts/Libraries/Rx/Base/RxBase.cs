using R3;
using Redbean.Core;

namespace Redbean.Base
{
	public class RxBase : Singleton
	{
		protected readonly CompositeDisposable disposables = new();
		
		public override void Dispose()
		{
			disposables.Dispose();
			disposables.Clear();
		}
	}
}