using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiPostRequest : ApiBase
	{
		public static async Task<Response<AppVersionResponse>> PostAppVersionRequest(AppVersionRequest args) =>
			await SendPostRequest<Response<AppVersionResponse>>("/Config/PostAppVersion", args);

		public static async Task<Response<StringArrayResponse>> PostTableFilesRequest(params object[] args) =>
			await SendPostRequest<Response<StringArrayResponse>>("/Storage/PostTableFiles", args);

		public static async Task<Response<StringArrayResponse>> PostBundleFilesRequest(params object[] args) =>
			await SendPostRequest<Response<StringArrayResponse>>("/Storage/PostBundleFiles?type={0}", args);

		public static async Task<Response<UserResponse>> PostUserNicknameRequest(params object[] args) =>
			await SendPostRequest<Response<UserResponse>>("/User/PostUserNickname?nickname={0}", args);
	}
}
