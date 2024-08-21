using System.Threading;
using System.Threading.Tasks;
using Redbean.Table;

namespace Redbean.Api
{
	public class GetTableConfigProtocol : ApiProtocol
	{
		public override async Task<object> RequestAsync(CancellationToken cancellationToken = default)
		{
			var request = await ApiGetRequest.GetTableConfigRequest();
			if (request.ErrorCode > 0)
				return request.Response;
			
			GoogleTableSettings.SheetId = request.Response.Sheet.Id;
			GoogleTableSettings.ClientId = request.Response.Client.Id;
			GoogleTableSettings.ClientSecretId = request.Response.Client.Secret;
			
			return request.Response;
		}
	}
}