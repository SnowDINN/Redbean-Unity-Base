using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Redbean.Bundle
{
	public class AddressableBootstrap : IApplicationBootstrap
	{
		public int ExecutionOrder => 300;

		public async Task Setup()
		{
			await Addressables.InitializeAsync().Task;
		}

		public void Dispose()
		{
			
		}
	}
}