using System.Linq;
using System.Threading.Tasks;
using Redbean.Table;

namespace Redbean.Api
{
	public class GetTableProtocol : IApiContainer
	{
		public async Task<object> Request(params object[] args)
		{
			var request = await ApiGetRequest.GetTableRequest();
			if (request.ErrorCode > 0)
				return request.Response;
			
			if (request.Response.Table.Any())
				TableContainer.RawTable = request.Response.Table;
			
			return request.Response;
		}
	}
}