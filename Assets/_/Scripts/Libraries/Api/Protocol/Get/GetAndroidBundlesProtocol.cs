using System.Threading.Tasks;

namespace Redbean.Api
{
	public class GetAndroidBundlesProtocol : IApi
	{
		public async Task<Response> Request(params object[] args)
		{
			return await ApiGetRequest.GetAndroidBundleFilesRequest(ApplicationSettings.Version);
		}
	}
}