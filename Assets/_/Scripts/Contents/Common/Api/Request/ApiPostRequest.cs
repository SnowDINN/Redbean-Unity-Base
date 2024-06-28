using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiPostRequest : ApiBase
	{
		public static async Task<AppVersionResponse> PostAppVersionRequest(params object[] args) =>
			await SendPostRequest<AppVersionResponse>("/Config/PostAppVersion?version={0}&type={1}", args);

		public static async Task<StringArrayResponse> PostTableFilesRequest(params object[] args) =>
			await SendPostRequest<StringArrayResponse>("/Storage/PostTableFiles", args);

		public static async Task<StringArrayResponse> PostBundleFilesRequest(params object[] args) =>
			await SendPostRequest<StringArrayResponse>("/Storage/PostBundleFiles?type={0}", args);

		public static async Task<UserResponse> PostUserNicknameRequest(params object[] args) =>
			await SendPostRequest<UserResponse>("/User/PostUserNickname?nickname={0}", args);
	}
}
