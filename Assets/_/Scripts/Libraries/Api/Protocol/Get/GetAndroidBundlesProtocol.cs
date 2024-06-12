using System.Threading.Tasks;
using Redbean.Api;

namespace Redbean.Api
{
	public class GetAndroidBundlesProtocol : IApi
	{
		public async Task Request(params object[] parameters)
		{
			var request = await ApiGetRequest.GetAndroidBundleFilesRequest(ApplicationSettings.Version);

			return;
		}
	}
}