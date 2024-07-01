using System.Threading.Tasks;

namespace Redbean.Api
{
	public class PostTableFileProtocol : IApiContainer
	{
		public async Task<object> Request(params object[] args)
		{
			return (await ApiPostRequest.PostTableFilesRequest(new AppUploadFilesRequest
			{
				Files = args as RequestFile[]
			})).Response;
		}
	}
}