using System.Threading.Tasks;

namespace Redbean.Api
{
	public class GetBundlesProtocol : IApi
	{
		public async Task<Response> Request(params object[] args)
		{
			return await ApiGetRequest.GetBundleFilesRequest(AppSettings.Version, AppSettings.MobileType);
		}
	}
}