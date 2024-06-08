using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiDeleteProtocol : ApiBase
	{
		public static async Task<ResponseResult> DeleteTablesRequest(params string[] parameters) =>
			await SendGetRequest("https://localhost:44395/Storage/DeleteTables?version={0}", parameters);

		public static async Task<ResponseResult> DeleteAndroidBundlesRequest(params string[] parameters) =>
			await SendGetRequest("https://localhost:44395/Storage/DeleteAndroidBundles?version={0}", parameters);

		public static async Task<ResponseResult> DeleteiOSBundlesRequest(params string[] parameters) =>
			await SendGetRequest("https://localhost:44395/Storage/DeleteiOSBundles?version={0}", parameters);
	}
}
