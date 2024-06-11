using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiPostRequest : ApiBase
	{
		public static async Task<Response> PostAndroidVersionRequest(params object[] parameters) =>
			await SendPostRequest("https://localhost:44395/Config/PostAndroidVersion?version={0}", parameters);

		public static async Task<Response> PostiOSVersionRequest(params object[] parameters) =>
			await SendPostRequest("https://localhost:44395/Config/PostiOSVersion?version={0}", parameters);
	}
}
