using System.Net.Http;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class PostiOSBundleFileProtocol : IApi
	{
		public async Task<Response> Request(params object[] args)
		{
			return await ApiPostRequest.PostiOSBundleFileRequest(ApplicationSettings.Version, args[0] as HttpContent);
		}
	}
}