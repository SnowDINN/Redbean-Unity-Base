using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiPostRequest : ApiBase
	{
		public static async Task<Response<AppVersionResponse>> PostAppVersionRequest(AppVersionRequest args) =>
			await SendPostRequest<Response<AppVersionResponse>>("/Config/PostAppVersion", args);

		public static async Task<Response<StringArrayResponse>> PostTableFilesRequest(AppUploadFilesRequest args) =>
			await SendPostRequest<Response<StringArrayResponse>>("/Storage/PostTableFiles", args);

		public static async Task<Response<StringArrayResponse>> PostBundleFilesRequest(AppUploadFilesRequest args) =>
			await SendPostRequest<Response<StringArrayResponse>>("/Storage/PostBundleFiles", args);

		public static async Task<Response<UserResponse>> PostUserNicknameRequest(params object[] args) =>
			await SendPostRequest<Response<UserResponse>>("/User/PostUserNickname?nickname={0}", args);
	}
}
