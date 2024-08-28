using System.Threading;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiGetRequest : ApiBase
	{
		public static async Task<ApiResponse<AppConfigResponse>> GetAppConfigRequest(object[] args = default, CancellationToken cancellationToken = default) =>
			await GetRequestAsync<ApiResponse<AppConfigResponse>>("/Setting/GetAppConfig", args, cancellationToken);

		public static async Task<ApiResponse<TableConfigResponse>> GetTableConfigRequest(object[] args = default, CancellationToken cancellationToken = default) =>
			await GetRequestAsync<ApiResponse<TableConfigResponse>>("/Setting/GetTableConfig", args, cancellationToken);

		public static async Task<ApiResponse<TableResponse>> GetTableRequest(object[] args = default, CancellationToken cancellationToken = default) =>
			await GetRequestAsync<ApiResponse<TableResponse>>("/Setting/GetTable", args, cancellationToken);

#if UNITY_EDITOR
#endif
	}
}
