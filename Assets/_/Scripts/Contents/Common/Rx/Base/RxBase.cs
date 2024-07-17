﻿using R3;

namespace Redbean.Rx
{
	public class RxBase
	{
		protected readonly CompositeDisposable disposables = new();

		public virtual void Dispose()
		{
			disposables?.Dispose();
			disposables?.Clear();
		}
	}
}