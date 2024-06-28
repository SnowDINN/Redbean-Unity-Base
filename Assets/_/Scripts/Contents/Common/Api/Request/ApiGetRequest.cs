using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiGetRequest : ApiBase
	{
		public static async Task<Response> GetUserRequest(params object[] args) =>
			await SendGetRequest("/Authentication/GetUser?id={0}", args);

		public static async Task<Response> GetEditorAccessTokenRequest(params object[] args) =>
			await SendGetRequest("/Authentication/GetEditorAccessToken?email={0}", args);

		public static async Task<Response> GetRefreshAccessTokenRequest(params object[] args) =>
			await SendGetRequest("/Authentication/GetRefreshAccessToken?refreshToken={0}", args);

		public static async Task<Response> GetAppConfigRequest(params object[] args) =>
			await SendGetRequest("/Common/GetAppConfig", args);

		public static async Task<Response> GetTableRequest(params object[] args) =>
			await SendGetRequest("/Common/GetTable", args);

		public static async Task<Response> GetTableConfigRequest(params object[] args) =>
			await SendGetRequest("/Config/GetTableConfig", args);
	}
}
