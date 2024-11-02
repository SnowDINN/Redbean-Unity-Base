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

			var googleSheetScriptable = ApplicationLoader.GetScriptable<GoogleSheetScriptable>();
			googleSheetScriptable.GoogleSheetId = response.Response.Sheet.Id;
			googleSheetScriptable.GoogleClientId = response.Response.Client.Id;
			googleSheetScriptable.GoogleSecretId = response.Response.Client.Secret;
			
			return response;
		}
	}
}