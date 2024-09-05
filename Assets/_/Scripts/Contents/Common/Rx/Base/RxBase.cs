using System.Threading;
using R3;

namespace Redbean.Rx
{
	public class RxBase : IExtension
	{
		private readonly CancellationTokenSource source = new();
		protected CancellationToken cancellationToken => source.Token;
		
		protected readonly CompositeDisposable disposables = new();

		public virtual void Setup()
		{
		}

		public virtual void Teardown()
		{
			disposables?.Dispose();
			disposables?.Clear();
			
			source?.Cancel();
			source?.Dispose();
		}
	}
}