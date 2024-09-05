using System.Threading;
using System.Threading.Tasks;
using Redbean.Table;

namespace Redbean.Api
{
	public class EditTableAccessKeyProtocol : ApiProtocol
	{
		protected override async Task<ApiResponse> Request(CancellationToken cancellationToken = default)
		{
			var response = await ApiGetRequest.EditTableAccessKeyRequest(cancellationToken: cancellationToken);
			if (!response.IsSuccess)
				return response;
			
			GoogleTableSettings.SheetId = response.Response.Sheet.Id;
			GoogleTableSettings.ClientId = response.Response.Client.Id;
			GoogleTableSettings.ClientSecretId = response.Response.Client.Secret;
			
			return response;
		}
	}
}