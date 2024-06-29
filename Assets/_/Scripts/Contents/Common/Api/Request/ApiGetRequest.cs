using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiGetRequest : ApiBase
	{
		public static async Task<Response<UserAndTokenResponse>> GetUserRequest(params object[] args) =>
			await SendGetRequest<Response<UserAndTokenResponse>>("/Authentication/GetUser?id={0}", args);

		public static async Task<Response<StringResponse>> GetEditorAccessTokenRequest(params object[] args) =>
			await SendGetRequest<Response<StringResponse>>("/Authentication/GetEditorAccessToken?email={0}", args);

		public static async Task<Response<TokenResponse>> GetRefreshAccessTokenRequest(params object[] args) =>
			await SendGetRequest<Response<TokenResponse>>("/Authentication/GetRefreshAccessToken?refreshToken={0}", args);

		public static async Task<Response<AppConfigResponse>> GetAppConfigRequest(params object[] args) =>
			await SendGetRequest<Response<AppConfigResponse>>("/Common/GetAppConfig", args);

		public static async Task<Response<DictionaryResponse>> GetTableRequest(params object[] args) =>
			await SendGetRequest<Response<DictionaryResponse>>("/Common/GetTable", args);

		public static async Task<Response<TableConfigResponse>> GetTableConfigRequest(params object[] args) =>
			await SendGetRequest<Response<TableConfigResponse>>("/Config/GetTableConfig", args);
	}
}
