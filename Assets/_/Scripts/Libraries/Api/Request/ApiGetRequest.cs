using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiGetRequest : ApiBase
	{
		public static async Task<Response> GetUserRequest(params object[] args) =>
			await SendGetRequest("/Authentication/GetUser?uid={0}", args);

		public static async Task<Response> GetAppConfigRequest(params object[] args) =>
			await SendGetRequest("/Config/GetAppConfig", args);

		public static async Task<Response> GetTableConfigRequest(params object[] args) =>
			await SendGetRequest("/Config/GetTableConfig", args);

		public static async Task<Response> GetTableRequest(params object[] args) =>
			await SendGetRequest("/Storage/GetTable?version={0}", args);
	}
}
