using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiGetRequest : ApiBase
	{
		public static async Task<Response> GetUserRequest(params object[] args) =>
			await SendGetRequest(ApplicationSettings.ApiUri + "/Authentication/GetUser?uid={0}", args);

		public static async Task<Response> GetApplicationConfigRequest(params object[] args) =>
			await SendGetRequest(ApplicationSettings.ApiUri + "/Config/GetApplicationConfig", args);

		public static async Task<Response> GetTableConfigRequest(params object[] args) =>
			await SendGetRequest(ApplicationSettings.ApiUri + "/Config/GetTableConfig", args);

		public static async Task<Response> GetTableFilesRequest(params object[] args) =>
			await SendGetRequest(ApplicationSettings.ApiUri + "/Storage/GetTableFiles?version={0}", args);

		public static async Task<Response> GetAndroidBundleFilesRequest(params object[] args) =>
			await SendGetRequest(ApplicationSettings.ApiUri + "/Storage/GetAndroidBundleFiles?version={0}", args);

		public static async Task<Response> GetiOSBundleFilesRequest(params object[] args) =>
			await SendGetRequest(ApplicationSettings.ApiUri + "/Storage/GetiOSBundleFiles?version={0}", args);
	}
}
