﻿using R3;
using Redbean.Core;

namespace Redbean.Base
{
	public class RxBase : ISingleton
	{
		protected readonly CompositeDisposable disposables = new();
	}
}