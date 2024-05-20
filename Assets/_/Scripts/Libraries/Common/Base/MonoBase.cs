using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Redbean.Base
{
	public class MonoBase : MonoBehaviour, IDisposable
	{
		private readonly Dictionary<string, CancellationTokenSource> Cancellations = new();
		public CancellationTokenSource DestroyCancellation = new();

		/// <summary>
		/// 토큰 갱신
		/// </summary>
		public CancellationTokenSource GenerateCancellationToken(string tokenName)
		{
			if (Cancellations.TryGetValue(tokenName, out var cancellation))
				cancellation.CancelAndDispose();
			Cancellations[tokenName] = new CancellationTokenSource();

			return Cancellations[tokenName];
		}

		public virtual void OnDestroy()
		{
			Dispose();
		}

		public void Dispose()
		{
			foreach (var cancellation in Cancellations)
				Cancellations[cancellation.Key].CancelAndDispose();
			
			DestroyCancellation?.CancelAndDispose();
		}
	}	
}