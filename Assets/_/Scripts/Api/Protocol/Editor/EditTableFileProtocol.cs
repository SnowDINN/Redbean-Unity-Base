using System.Threading;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class EditTableFileProtocol : ApiProtocol
	{
		protected override async Task<ApiResponse> Request(CancellationToken cancellationToken = default)
		{
			return await ApiPostRequest.EditTableFilesRequest(new AppUploadFilesRequest
			{
				Files = args as RequestFile[]
			}, cancellationToken);
		}
	}
}