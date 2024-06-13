using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiPostRequest : ApiBase
	{
		public static async Task<Response> PostAndroidVersionRequest(params object[] args) =>
			await SendPostRequest(ApplicationSettings.ApiUri + "/Config/PostAndroidVersion?version={0}", args);

		public static async Task<Response> PostiOSVersionRequest(params object[] args) =>
			await SendPostRequest(ApplicationSettings.ApiUri + "/Config/PostiOSVersion?version={0}", args);

		public static async Task<Response> PostTableFileRequest(params object[] args) =>
			await SendPostRequest(ApplicationSettings.ApiUri + "/Storage/PostTableFile?version={0}", args);

		public static async Task<Response> PostAndroidBundleFileRequest(params object[] args) =>
			await SendPostRequest(ApplicationSettings.ApiUri + "/Storage/PostAndroidBundleFile?version={0}", args);

		public static async Task<Response> PostiOSBundleFileRequest(params object[] args) =>
			await SendPostRequest(ApplicationSettings.ApiUri + "/Storage/PostiOSBundleFile?version={0}", args);
	}
}
