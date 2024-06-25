using System.Threading.Tasks;

namespace Redbean.Api
{
	public class PostAppVersionProtocol : IApiContainer
	{
		public async Task<Response> Request(params object[] args)
		{
			return await ApiPostRequest.PostAppVersionRequest(args[0], args[1]);
		}
	}
}