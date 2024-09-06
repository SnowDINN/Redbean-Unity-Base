using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Redbean.Table;

namespace Redbean.Api
{
	public class GetTableSettingProtocol : ApiProtocol
	{
		protected override async Task<ApiResponse> Request(CancellationToken cancellationToken = default)
		{
			var response = await ApiGetRequest.GetTableSettingRequest(cancellationToken: cancellationToken);
			if (!response.IsSuccess)
				return response;
			
			if (!response.Response.Table.Any())
				return response;

			TableContainer.SetRawData(response.Response.Table);
			TableContainer.Setup(nameof(TLocalization));

			return response;
		}
	}
}