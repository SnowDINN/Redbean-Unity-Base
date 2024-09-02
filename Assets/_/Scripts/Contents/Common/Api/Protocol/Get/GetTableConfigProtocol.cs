using System.Threading;
using System.Threading.Tasks;
using Redbean.Table;

namespace Redbean.Api
{
	public class GetTableConfigProtocol : ApiProtocol
	{
		protected override async Task<IApiResponse> Request(CancellationToken cancellationToken = default)
		{
			var request = await ApiGetRequest.GetTableConfigRequest(cancellationToken: cancellationToken);
			if (request.ErrorCode != 0)
				return request.Response;
			
			GoogleTableSettings.SheetId = request.Response.Sheet.Id;
			GoogleTableSettings.ClientId = request.Response.Client.Id;
			GoogleTableSettings.ClientSecretId = request.Response.Client.Secret;
			
			return request.Response;
		}
	}
}