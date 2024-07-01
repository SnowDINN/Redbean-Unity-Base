using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiGetRequest : ApiBase
	{
		public static async Task<ApiResponse<UserAndTokenResponse>> GetUserRequest(params object[] args) =>
			await SendGetRequest<ApiResponse<UserAndTokenResponse>>("/Authentication/GetUser?id={0}", args);

		public static async Task<ApiResponse<StringResponse>> GetEditorAccessTokenRequest(params object[] args) =>
			await SendGetRequest<ApiResponse<StringResponse>>("/Authentication/GetEditorAccessToken?email={0}", args);

		public static async Task<ApiResponse<TokenResponse>> GetRefreshAccessTokenRequest(params object[] args) =>
			await SendGetRequest<ApiResponse<TokenResponse>>("/Authentication/GetRefreshAccessToken?refreshToken={0}", args);

		public static async Task<ApiResponse<AppConfigResponse>> GetAppConfigRequest(params object[] args) =>
			await SendGetRequest<ApiResponse<AppConfigResponse>>("/Common/GetAppConfig", args);

		public static async Task<ApiResponse<TableResponse>> GetTableRequest(params object[] args) =>
			await SendGetRequest<ApiResponse<TableResponse>>("/Common/GetTable", args);

		public static async Task<ApiResponse<TableConfigResponse>> GetTableConfigRequest(params object[] args) =>
			await SendGetRequest<ApiResponse<TableConfigResponse>>("/Config/GetTableConfig", args);
	}
}
