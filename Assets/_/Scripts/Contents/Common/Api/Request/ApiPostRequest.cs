using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiPostRequest : ApiBase
	{
		public static async Task<ApiResponse<StringResponse>> PostEditorAccessTokenRequest(StringRequest args) =>
			await SendPostRequest<ApiResponse<StringResponse>>("/Authentication/PostEditorAccessToken", args);

		public static async Task<ApiResponse<AppVersionResponse>> PostAppVersionRequest(AppVersionRequest args) =>
			await SendPostRequest<ApiResponse<AppVersionResponse>>("/Config/PostAppVersion", args);

		public static async Task<ApiResponse<AppConfigResponse>> PostAppMaintenanceRequest(AppMaintenanceRequest args) =>
			await SendPostRequest<ApiResponse<AppConfigResponse>>("/Config/PostAppMaintenance", args);

		public static async Task<ApiResponse<StringArrayResponse>> PostTableFilesRequest(AppUploadFilesRequest args) =>
			await SendPostRequest<ApiResponse<StringArrayResponse>>("/Storage/PostTableFiles", args);

		public static async Task<ApiResponse<StringArrayResponse>> PostBundleFilesRequest(AppUploadFilesRequest args) =>
			await SendPostRequest<ApiResponse<StringArrayResponse>>("/Storage/PostBundleFiles", args);

		public static async Task<ApiResponse<UserResponse>> PostUserNicknameRequest(StringRequest args) =>
			await SendPostRequest<ApiResponse<UserResponse>>("/User/PostUserNickname", args);
	}
}
