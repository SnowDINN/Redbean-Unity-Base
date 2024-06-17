using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiPostRequest : ApiBase
	{
		public static async Task<Response> PostAppVersionRequest(params object[] args) =>
			await SendPostRequest("/Config/PostAppVersion?version={0}&type={1}", args);

		public static async Task<Response> PostTableFilesRequest(params object[] args) =>
			await SendPostRequest("/Storage/PostTableFiles?version={0}", args);

		public static async Task<Response> PostBundleFilesRequest(params object[] args) =>
			await SendPostRequest("/Storage/PostBundleFiles?version={0}&type={1}", args);
	}
}
