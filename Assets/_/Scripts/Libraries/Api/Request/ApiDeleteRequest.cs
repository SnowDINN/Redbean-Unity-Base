using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiDeleteRequest : ApiBase
	{
		public static async Task<Response> DeleteTableFilesRequest(params object[] parameters) =>
			await SendDeleteRequest("https://localhost:44395/Storage/DeleteTableFiles?version={0}", parameters);

		public static async Task<Response> DeleteAndroidBundleFilesRequest(params object[] parameters) =>
			await SendDeleteRequest("https://localhost:44395/Storage/DeleteAndroidBundleFiles?version={0}", parameters);

		public static async Task<Response> DeleteiOSBundleFilesRequest(params object[] parameters) =>
			await SendDeleteRequest("https://localhost:44395/Storage/DeleteiOSBundleFiles?version={0}", parameters);
	}
}
