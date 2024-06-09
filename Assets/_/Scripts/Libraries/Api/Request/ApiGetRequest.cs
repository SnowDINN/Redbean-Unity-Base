using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiGetRequest : ApiBase
	{
		public static async Task<ResponseResult> GetUserRequest(params object[] parameters) =>
			await SendGetRequest("https://localhost:44395/Authentication/GetUser?uid={0}", parameters);

		public static async Task<ResponseResult> GetApplicationConfigRequest(params object[] parameters) =>
			await SendGetRequest("https://localhost:44395/Config/GetApplicationConfig?", parameters);

		public static async Task<ResponseResult> GetTableConfigRequest(params object[] parameters) =>
			await SendGetRequest("https://localhost:44395/Config/GetTableConfig?", parameters);

		public static async Task<ResponseResult> GetTableFilesRequest(params object[] parameters) =>
			await SendGetRequest("https://localhost:44395/Storage/GetTableFiles?version={0}", parameters);

		public static async Task<ResponseResult> GetAndroidBundleFilesRequest(params object[] parameters) =>
			await SendGetRequest("https://localhost:44395/Storage/GetAndroidBundleFiles?version={0}", parameters);

		public static async Task<ResponseResult> GetiOSBundleFilesRequest(params object[] parameters) =>
			await SendGetRequest("https://localhost:44395/Storage/GetiOSBundleFiles?version={0}", parameters);
	}
}
