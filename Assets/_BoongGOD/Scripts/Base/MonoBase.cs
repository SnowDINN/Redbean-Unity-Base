using System;
using System.Threading;
using UnityEngine;

public class MonoBase : MonoBehaviour, IDisposable
{
	protected CancellationTokenSource DestroyCancellation;

	protected CancellationTokenSource CancellationTokenRefresh()
	{
		DestroyCancellation?.CancelAndDispose();
		DestroyCancellation = new CancellationTokenSource();

		return DestroyCancellation;
	}

	protected virtual void OnDestroy()
	{
		Dispose();
	}

	public void Dispose()
	{
		DestroyCancellation?.CancelAndDispose();
	}
}
