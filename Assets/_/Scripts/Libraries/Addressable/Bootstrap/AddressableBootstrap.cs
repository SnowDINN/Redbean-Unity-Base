using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Redbean.Api;
using UnityEngine.AddressableAssets;

namespace Redbean.Bundle
{
	public class AddressableBootstrap : IApplicationBootstrap
	{
		public int ExecutionOrder => 300;

		public async Task Setup()
		{
			var request = new ResponseResult();
			
#if UNITY_ANDROID
			request = await ApiGetRequest.GetAndroidBundleFilesRequest(ApplicationSettings.Version);
#endif
			
#if UNITY_IOS
			request = await ApiGetRequest.GetiOSBundleFilesRequest(ApplicationSettings.Version);
#endif
			
			var bundles = request.ToConvert<List<string>>();
			if (!bundles.Any())
			{
				Log.Fail("Bundle", "Fail to load to the bundles.");
				return;
			}
			
			var size = 0L;
			foreach (var label in AddressableSettings.Labels)
				size += await Addressables.GetDownloadSizeAsync(label).Task;

			if (size > 0)
			{
				foreach (var label in AddressableSettings.Labels)
				{
					await Addressables.DownloadDependenciesAsync(label).Task;
					Log.Notice($"{label} bundle load is complete.");
				}	
			}

			var convert = ConvertDownloadSize(size);
			Log.Success("Bundle", $"Success to load to the bundles. [ {convert.value}{convert.type} ]");
		}

		public void Dispose()
		{
			Log.System("Bundle has been terminated.");
		}

		private static (string value, string type) ConvertDownloadSize(long size)
		{
			var value = (double)size;
			value /= 1024;
			if (value < 1024)
				return ($"{value:F1}", "KB");
			
			value /= 1024;
			if (value < 1024)
				return ($"{value:F1}", "MB");
			
			value /= 1024;
			return ($"{value:F1}", "GB");
		}
	}
}