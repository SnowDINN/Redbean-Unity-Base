using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiGetRequest : ApiBase
	{
		public static async Task<UserAndTokenResponse> GetUserRequest(params object[] args) =>
			await SendGetRequest<UserAndTokenResponse>("/Authentication/GetUser?id={0}", args);

		public static async Task<StringResponse> GetEditorAccessTokenRequest(params object[] args) =>
			await SendGetRequest<StringResponse>("/Authentication/GetEditorAccessToken?email={0}", args);

		public static async Task<TokenResponse> GetRefreshAccessTokenRequest(params object[] args) =>
			await SendGetRequest<TokenResponse>("/Authentication/GetRefreshAccessToken?refreshToken={0}", args);

		public static async Task<AppConfigResponse> GetAppConfigRequest(params object[] args) =>
			await SendGetRequest<AppConfigResponse>("/Common/GetAppConfig", args);

		public static async Task<DictionaryResponse> GetTableRequest(params object[] args) =>
			await SendGetRequest<DictionaryResponse>("/Common/GetTable", args);

		public static async Task<TableConfigResponse> GetTableConfigRequest(params object[] args) =>
			await SendGetRequest<TableConfigResponse>("/Config/GetTableConfig", args);
	}
}
