using System.Threading.Tasks;

namespace Redbean.Api
{
	public class PostBundleFilesProtocol : IApi
	{
		public async Task<Response> Request(params object[] args)
		{
			return await ApiPostRequest.PostBundleFilesRequest(AppSettings.Version, AppSettings.MobileType, args[0]);
		}
	}
}