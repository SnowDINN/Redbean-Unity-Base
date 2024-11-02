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

			GoogleSheetContainer.SetRawData(response.Response.Table);
			GoogleSheetContainer.Setup(nameof(TLocalization));

			return response;
		}
	}
}