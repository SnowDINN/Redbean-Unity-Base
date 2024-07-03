using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiPostRequest : ApiBase
	{
		public static async Task<ApiResponse<StringResponse>> PostAppAccessTokenRequest(StringRequest args) =>
			await SendPostRequest<ApiResponse<StringResponse>>("/EditAccess/PostAppAccessToken", args);

		public static async Task<ApiResponse> PostAppVersionRequest(AppVersionRequest args) =>
			await SendPostRequest<ApiResponse>("/EditConfig/PostAppVersion", args);

		public static async Task<ApiResponse> PostAppMaintenanceRequest(AppMaintenanceRequest args) =>
			await SendPostRequest<ApiResponse>("/EditConfig/PostAppMaintenance", args);

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
