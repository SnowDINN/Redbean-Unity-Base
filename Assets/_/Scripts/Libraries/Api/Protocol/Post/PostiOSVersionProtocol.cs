using System.Threading.Tasks;

namespace Redbean.Api
{
	public class PostiOSVersionProtocol : IApi
	{
		public async Task<Response> Request(params object[] args)
		{
			return await ApiPostRequest.PostiOSVersionRequest(args);
		}
	}
}