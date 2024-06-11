using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiGetRequest : ApiBase
	{
		public static async Task<Response> GetUserRequest(params object[] parameters) =>
			await SendGetRequest("https://localhost:44395/Authentication/GetUser?uid={0}", parameters);

		public static async Task<Response> GetApplicationConfigRequest(params object[] parameters) =>
			await SendGetRequest("https://localhost:44395/Config/GetApplicationConfig?", parameters);

		public static async Task<Response> GetTableConfigRequest(params object[] parameters) =>
			await SendGetRequest("https://localhost:44395/Config/GetTableConfig?", parameters);

		public static async Task<Response> GetTableFilesRequest(params object[] parameters) =>
			await SendGetRequest("https://localhost:44395/Storage/GetTableFiles?version={0}", parameters);

		public static async Task<Response> GetAndroidBundleFilesRequest(params object[] parameters) =>
			await SendGetRequest("https://localhost:44395/Storage/GetAndroidBundleFiles?version={0}", parameters);

		public static async Task<Response> GetiOSBundleFilesRequest(params object[] parameters) =>
			await SendGetRequest("https://localhost:44395/Storage/GetiOSBundleFiles?version={0}", parameters);
	}
}
