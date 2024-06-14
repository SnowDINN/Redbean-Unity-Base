using System.Threading.Tasks;

namespace Redbean.Api
{
	public class PostTableFileProtocol : IApi
	{
		public async Task<Response> Request(params object[] args)
		{
			return await ApiPostRequest.PostTableFilesRequest(AppSettings.Version, args[0]);
		}
	}
}