using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiPostRequest : ApiBase
	{
		public static async Task<Response> PostAndroidVersionRequest(params object[] args) =>
			await SendPostRequest("https://localhost:44395/Config/PostAndroidVersion?version={0}", args);

		public static async Task<Response> PostiOSVersionRequest(params object[] args) =>
			await SendPostRequest("https://localhost:44395/Config/PostiOSVersion?version={0}", args);

		public static async Task<Response> PostTableFilesRequest(params object[] args) =>
			await SendPostRequest("https://localhost:44395/Storage/PostTableFiles?version={0}", args);

		public static async Task<Response> PostAndroidBundleFilesRequest(params object[] args) =>
			await SendPostRequest("https://localhost:44395/Storage/PostAndroidBundleFiles?version={0}", args);

		public static async Task<Response> PostiOSBundleFilesRequest(params object[] args) =>
			await SendPostRequest("https://localhost:44395/Storage/PostiOSBundleFiles?version={0}", args);
	}
}
