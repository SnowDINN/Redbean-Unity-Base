using System;
using R3;

namespace Redbean.Base
{
	public class RxBase : IDisposable
	{
		protected readonly CompositeDisposable disposables = new();
		
		public void Dispose()
		{
			disposables.Dispose();
			disposables.Clear();
		}
	}
}