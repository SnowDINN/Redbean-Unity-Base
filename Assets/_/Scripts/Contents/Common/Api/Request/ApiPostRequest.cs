using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiPostRequest : ApiBase
	{
		public static async Task<ApiResponse<StringResponse>> PostEditorAccessTokenRequest(StringRequest args) =>
			await SendPostRequest<ApiResponse<StringResponse>>("/EditAccess/PostEditorAccessToken", args);

		public static async Task<ApiResponse<AppVersionResponse>> PostAppVersionRequest(AppVersionRequest args) =>
			await SendPostRequest<ApiResponse<AppVersionResponse>>("/EditConfig/PostAppVersion", args);

		public static async Task<ApiResponse<AppConfigResponse>> PostAppMaintenanceRequest(AppMaintenanceRequest args) =>
			await SendPostRequest<ApiResponse<AppConfigResponse>>("/EditConfig/PostAppMaintenance", args);

		public static async Task<ApiResponse<StringArrayResponse>> PostTableFilesRequest(AppUploadFilesRequest args) =>
			await SendPostRequest<ApiResponse<StringArrayResponse>>("/EditFiles/PostTableFiles", args);

		public static async Task<ApiResponse<StringArrayResponse>> PostBundleFilesRequest(AppUploadFilesRequest args) =>
			await SendPostRequest<ApiResponse<StringArrayResponse>>("/EditFiles/PostBundleFiles", args);

		public static async Task<ApiResponse> PostUserNicknameRequest(StringRequest args) =>
			await SendPostRequest<ApiResponse>("/User/PostUserNickname", args);

		public static async Task<ApiResponse> PostUserWithdrawalRequest(params object[] args) =>
			await SendPostRequest<ApiResponse>("/User/PostUserWithdrawal", args);
	}
}
