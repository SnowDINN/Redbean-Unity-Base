using System.Threading.Tasks;

namespace Redbean.Api
{
	public class PostAndroidVersionProtocol : IApi
	{
		public async Task<Response> Request(params object[] args)
		{
			return await ApiPostRequest.PostAndroidVersionRequest();
		}
	}
}