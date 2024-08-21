using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Redbean.Table;

namespace Redbean.Api
{
	public class GetTableProtocol : ApiProtocol
	{
		public override async Task<object> RequestAsync(CancellationToken cancellationToken = default)
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