using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiGetProtocol : ApiBase
	{
		public static async Task<ResponseResult> GetUserRequest(params string[] parameters) =>
			await SendGetRequest("https://localhost:44395/Authentication/GetUser?uid={0}", parameters);

		public static async Task<ResponseResult> GetTablesRequest(params string[] parameters) =>
			await SendGetRequest("https://localhost:44395/Storage/GetTables?version={0}", parameters);

		public static async Task<ResponseResult> GetAndroidBundlesRequest(params string[] parameters) =>
			await SendGetRequest("https://localhost:44395/Storage/GetAndroidBundles?version={0}", parameters);

		public static async Task<ResponseResult> GetiOSBundlesRequest(params string[] parameters) =>
			await SendGetRequest("https://localhost:44395/Storage/GetiOSBundles?version={0}", parameters);
	}
}
