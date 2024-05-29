using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Redbean.Bundle
{
	public class AddressableBootstrap : IApplicationBootstrap
	{
		public int ExecutionOrder => 300;

		public async UniTask Setup()
		{
			await Addressables.InitializeAsync();
		}

		public void Dispose()
		{
			
		}
	}
}