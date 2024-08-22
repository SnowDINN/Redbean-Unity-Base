using System.Threading;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiGetRequest : ApiBase
	{
		public static async Task<ApiResponse<UserAndTokenResponse>> GetAccessTokenAndUserRequest(object[] args = default, CancellationToken cancellationToken = default) =>
			await GetRequestAsync<ApiResponse<UserAndTokenResponse>>("/GetAuthorize/GetAccessTokenAndUser?id={0}", args, cancellationToken);

		public static async Task<ApiResponse<TokenResponse>> GetAccessTokenRefreshRequest(object[] args = default, CancellationToken cancellationToken = default) =>
			await GetRequestAsync<ApiResponse<TokenResponse>>("/GetAuthorize/GetAccessTokenRefresh?token={0}", args, cancellationToken);

		public static async Task<ApiResponse<AppConfigResponse>> GetAppConfigRequest(object[] args = default, CancellationToken cancellationToken = default) =>
			await GetRequestAsync<ApiResponse<AppConfigResponse>>("/GetSetting/GetAppConfig", args, cancellationToken);

		public static async Task<ApiResponse<TableConfigResponse>> GetTableConfigRequest(object[] args = default, CancellationToken cancellationToken = default) =>
			await GetRequestAsync<ApiResponse<TableConfigResponse>>("/GetSetting/GetTableConfig", args, cancellationToken);

		public static async Task<ApiResponse<TableResponse>> GetTableRequest(object[] args = default, CancellationToken cancellationToken = default) =>
			await GetRequestAsync<ApiResponse<TableResponse>>("/GetSetting/GetTable", args, cancellationToken);

#if UNITY_EDITOR
#endif
	}
}
