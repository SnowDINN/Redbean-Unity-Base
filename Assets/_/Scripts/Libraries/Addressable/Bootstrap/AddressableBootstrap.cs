using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Redbean.Bundle
{
	public class AddressableBootstrap : IApplicationBootstrap
	{
		public int ExecutionOrder => 300;

		public async Task Setup()
		{
			var size = 0L;
			foreach (var label in AddressableSettings.Labels)
				size += await Addressables.GetDownloadSizeAsync(label).Task;
			
			foreach (var label in AddressableSettings.Labels)
				await Addressables.DownloadDependenciesAsync(label).Task;
			 
			 Log.Success("Bundle", $"Success to load to the bundles. [{size / 1024}KB]");
		}

		public void Dispose()
		{
			
		}
	}
}