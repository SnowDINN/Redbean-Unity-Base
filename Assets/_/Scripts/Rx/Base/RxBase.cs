using System.Threading;
using R3;

namespace Redbean.Rx
{
	public class RxBase : IExtension
	{
		private readonly CancellationTokenSource source = new();
		protected CancellationToken cancellationToken => source.Token;
		
		protected readonly CompositeDisposable disposables = new();

		public void Start()
		{
			AppLifeCycle.OnAppExit += OnApplicationExit;
			Setup();
		}
		
		private void OnApplicationExit()
		{
			AppLifeCycle.OnAppExit -= OnApplicationExit;
			Teardown();
			
			disposables?.Dispose();
			disposables?.Clear();
			source?.Cancel();
		}
		
		protected virtual void Setup() { }
		protected virtual void Teardown() { }
	}
}