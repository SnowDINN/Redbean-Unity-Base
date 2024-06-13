using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiDeleteRequest : ApiBase
	{
		public static async Task<Response> DeleteTableFileRequest(params object[] args) =>
			await SendDeleteRequest(ApplicationSettings.ApiUri + "/Storage/DeleteTableFile?version={0}", args);

		public static async Task<Response> DeleteAndroidBundleFileRequest(params object[] args) =>
			await SendDeleteRequest(ApplicationSettings.ApiUri + "/Storage/DeleteAndroidBundleFile?version={0}", args);

		public static async Task<Response> DeleteiOSBundleFileRequest(params object[] args) =>
			await SendDeleteRequest(ApplicationSettings.ApiUri + "/Storage/DeleteiOSBundleFile?version={0}", args);
	}
}
