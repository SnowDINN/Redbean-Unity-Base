using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Redbean.Table;

namespace Redbean.Api
{
	public class GetTableProtocol : ApiProtocol
	{
		protected override async Task<IApiResponse> Request(CancellationToken cancellationToken = default)
		{
			var request = await ApiGetRequest.GetTableRequest(cancellationToken: cancellationToken);
			if (request.ErrorCode != 0)
				return request.Response;

			if (request.Response.Table.Any())
				TableContainer.StartParsing(request.Response.Table);
			
			return request.Response;
		}
	}
}