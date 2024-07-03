using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiGetRequest : ApiBase
	{
		public static async Task<ApiResponse<UserAndTokenResponse>> GetAccessTokenAndUserRequest(params object[] args) =>
			await SendGetRequest<ApiResponse<UserAndTokenResponse>>("/GetAuthorize/GetAccessTokenAndUser?id={0}", args);

		public static async Task<ApiResponse<TokenResponse>> GetAccessTokenRefreshRequest(params object[] args) =>
			await SendGetRequest<ApiResponse<TokenResponse>>("/GetAuthorize/GetAccessTokenRefresh?token={0}", args);

		public static async Task<ApiResponse<AppConfigResponse>> GetAppConfigRequest(params object[] args) =>
			await SendGetRequest<ApiResponse<AppConfigResponse>>("/GetSetting/GetAppConfig", args);

		public static async Task<ApiResponse<TableConfigResponse>> GetTableConfigRequest(params object[] args) =>
			await SendGetRequest<ApiResponse<TableConfigResponse>>("/GetSetting/GetTableConfig", args);

		public static async Task<ApiResponse<TableResponse>> GetTableRequest(params object[] args) =>
			await SendGetRequest<ApiResponse<TableResponse>>("/GetSetting/GetTable", args);
	}
}
