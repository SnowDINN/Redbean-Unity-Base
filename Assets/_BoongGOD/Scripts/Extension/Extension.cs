using System.Threading;

public static class Extension
{
	public static void CancelAndDispose(this CancellationTokenSource cancellationTokenSource)
	{
		if (!cancellationTokenSource.IsCancellationRequested)
			cancellationTokenSource.Cancel();
		
		cancellationTokenSource.Dispose();
	}
}