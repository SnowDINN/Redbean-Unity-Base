using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Redbean.Table;

namespace Redbean.Api
{
	public class GetTableProtocol : ApiProtocol
	{
		protected override async Task<ApiResponse> Request(CancellationToken cancellationToken = default)
		{
			var response = await ApiGetRequest.GetTableRequest(cancellationToken: cancellationToken);
			if (!response.IsSuccess)
				return response;

			if (response.Response.Table.Any())
				TableContainer.SetTable(response.Response.Table);
			
			return response;
		}
	}
}