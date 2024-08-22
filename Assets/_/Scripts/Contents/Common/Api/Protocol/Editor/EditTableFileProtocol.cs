using System.Threading;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class EditTableFileProtocol : ApiProtocol
	{
		public override async Task<object> RequestAsync(CancellationToken cancellationToken = default)
		{
			return (await ApiPostRequest.EditTableFilesRequest(new AppUploadFilesRequest
			{
				Files = args as RequestFile[]
			}, cancellationToken)).Response;
		}
	}
}