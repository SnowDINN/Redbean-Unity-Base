using System.Threading.Tasks;

namespace Redbean.Api
{
	public class PostBundleFilesProtocol : IApiContainer
	{
		public async Task<Response> Request(params object[] args)
		{
			return await ApiPostRequest.PostBundleFilesRequest(AppSettings.PlatformType, args[0]);
		}
	}
}