using System.Threading;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class PostTableFileProtocol : ApiProtocol
	{
		public override async Task<object> RequestAsync(CancellationToken cancellationToken = default)
		{
			return (await ApiPostRequest.PostTableFilesRequest(new AppUploadFilesRequest
			{
				Files = args as RequestFile[]
			}, cancellationToken)).Response;
		}
	}
}