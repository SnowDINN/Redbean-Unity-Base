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
			
			GoogleSheetReferencer.SheetId = response.Response.Sheet.Id;
			GoogleSheetReferencer.ClientId = response.Response.Client.Id;
			GoogleSheetReferencer.ClientSecretId = response.Response.Client.Secret;
			
			return response;
		}
	}
}