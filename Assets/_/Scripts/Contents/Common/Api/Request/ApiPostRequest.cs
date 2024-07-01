using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiPostRequest : ApiBase
	{
		public static async Task<ApiResponse<AppVersionResponse>> PostAppVersionRequest(AppVersionRequest args) =>
			await SendPostRequest<ApiResponse<AppVersionResponse>>("/Config/PostAppVersion", args);

		public static async Task<ApiResponse<StringArrayResponse>> PostTableFilesRequest(AppUploadFilesRequest args) =>
			await SendPostRequest<ApiResponse<StringArrayResponse>>("/Storage/PostTableFiles", args);

		public static async Task<ApiResponse<StringArrayResponse>> PostBundleFilesRequest(AppUploadFilesRequest args) =>
			await SendPostRequest<ApiResponse<StringArrayResponse>>("/Storage/PostBundleFiles", args);

		public static async Task<ApiResponse<UserResponse>> PostUserNicknameRequest(params object[] args) =>
			await SendPostRequest<ApiResponse<UserResponse>>("/User/PostUserNickname?nickname={0}", args);
	}
}
