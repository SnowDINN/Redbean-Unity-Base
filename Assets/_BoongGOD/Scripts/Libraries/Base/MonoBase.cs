using System;
using System.Collections.Generic;
using System.Threading;
using Redbean.Extension;
using UnityEngine;

namespace Redbean.Base
{
	public class MonoBase : MonoBehaviour, IDisposable
	{
		protected Dictionary<string, CancellationTokenSource> Cancellations = new();
		protected CancellationTokenSource DestroyCancellation = new();

		/// <summary>
		/// 토큰 갱신
		/// </summary>
		protected CancellationTokenSource GenerateCancellationToken(string tokenName)
		{
			if (Cancellations.ContainsKey(tokenName))
				Cancellations[tokenName].CancelAndDispose();
			Cancellations[tokenName] = new CancellationTokenSource();

			return Cancellations[tokenName];
		}

		protected virtual void OnDestroy()
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