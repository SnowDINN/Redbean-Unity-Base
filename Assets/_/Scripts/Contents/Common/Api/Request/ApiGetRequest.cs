using System.Threading;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiGetRequest : ApiBase
	{
		public static async Task<ApiResponse<AppSettingResponse>> GetAppSettingRequest(object[] args = default, CancellationToken cancellationToken = default) =>
			await GetRequestAsync<ApiResponse<AppSettingResponse>>("/Setting/GetAppSetting", args, cancellationToken);

		public static async Task<ApiResponse<TableSettingResponse>> GetTableSettingRequest(object[] args = default, CancellationToken cancellationToken = default) =>
			await GetRequestAsync<ApiResponse<TableSettingResponse>>("/Setting/GetTableSetting", args, cancellationToken);

#if UNITY_EDITOR
		public static async Task<ApiResponse<TableAccessKeyResponse>> GetTableAccessKeyRequest(object[] args = default, CancellationToken cancellationToken = default) =>
			await GetRequestAsync<ApiResponse<TableAccessKeyResponse>>("/EditAccess/GetTableAccessKey", args, cancellationToken);
#endif
	}
}
