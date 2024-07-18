using System.Threading.Tasks;
using Redbean.Table;

namespace Redbean.Api
{
	public class GetTableConfigProtocol : IApiContainer
	{
		public async Task<object> Request(params object[] args)
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