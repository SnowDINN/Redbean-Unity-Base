using System;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Redbean.Bundle
{
	public class BundleBase<T> where T : Object
	{
		public T Value;

		public void Release() => Addressables.Release(Value);
	}
}