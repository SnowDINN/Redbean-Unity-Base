using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiPostRequest : ApiBase
	{
		public static async Task<Response> PostAppVersionRequest(params object[] args) =>
			await SendPostRequest("/Config/PostAppVersion?version={0}&type={1}", args);

		public static async Task<Response> PostTableFilesRequest(params object[] args) =>
			await SendPostRequest("/Storage/PostTableFiles", args);

		public static async Task<Response> PostBundleFilesRequest(params object[] args) =>
			await SendPostRequest("/Storage/PostBundleFiles?type={0}", args);

		public static async Task<Response> PostUserNicknameRequest(params object[] args) =>
			await SendPostRequest("/User/PostUserNickname?nickname={0}", args);
	}
}
