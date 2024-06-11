using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiGetRequest : ApiBase
	{
		public static async Task<Response> GetUserRequest(params object[] args) =>
			await SendGetRequest("https://localhost:44395/Authentication/GetUser?uid={0}", args);

		public static async Task<Response> GetApplicationConfigRequest(params object[] args) =>
			await SendGetRequest("https://localhost:44395/Config/GetApplicationConfig?", args);

		public static async Task<Response> GetTableConfigRequest(params object[] args) =>
			await SendGetRequest("https://localhost:44395/Config/GetTableConfig?", args);

		public static async Task<Response> GetTableFilesRequest(params object[] args) =>
			await SendGetRequest("https://localhost:44395/Storage/GetTableFiles?version={0}", args);

		public static async Task<Response> GetAndroidBundleFilesRequest(params object[] args) =>
			await SendGetRequest("https://localhost:44395/Storage/GetAndroidBundleFiles?version={0}", args);

		public static async Task<Response> GetiOSBundleFilesRequest(params object[] args) =>
			await SendGetRequest("https://localhost:44395/Storage/GetiOSBundleFiles?version={0}", args);
	}
}
