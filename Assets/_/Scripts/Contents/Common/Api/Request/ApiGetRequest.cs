using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiGetRequest : ApiBase
	{
		public static async Task<HttpResponse<UserAndTokenResponse>> GetUserRequest(params object[] args) =>
			await SendGetRequest<HttpResponse<UserAndTokenResponse>>("/Authentication/GetUser?id={0}", args);

		public static async Task<HttpResponse<StringResponse>> GetEditorAccessTokenRequest(params object[] args) =>
			await SendGetRequest<HttpResponse<StringResponse>>("/Authentication/GetEditorAccessToken?email={0}", args);

		public static async Task<HttpResponse<TokenResponse>> GetRefreshAccessTokenRequest(params object[] args) =>
			await SendGetRequest<HttpResponse<TokenResponse>>("/Authentication/GetRefreshAccessToken?refreshToken={0}", args);

		public static async Task<HttpResponse<AppConfigResponse>> GetAppConfigRequest(params object[] args) =>
			await SendGetRequest<HttpResponse<AppConfigResponse>>("/Common/GetAppConfig", args);

		public static async Task<HttpResponse<DictionaryResponse>> GetTableRequest(params object[] args) =>
			await SendGetRequest<HttpResponse<DictionaryResponse>>("/Common/GetTable", args);

		public static async Task<HttpResponse<TableConfigResponse>> GetTableConfigRequest(params object[] args) =>
			await SendGetRequest<HttpResponse<TableConfigResponse>>("/Config/GetTableConfig", args);
	}
}
