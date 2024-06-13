using System.Threading.Tasks;

namespace Redbean.Api
{
	public class PostTableFilesProtocol : IApi
	{
		public async Task<Response> Request(params object[] args)
		{
			return await ApiPostRequest.PostTableFilesRequest(args);
		}
	}
}