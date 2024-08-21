using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiPostRequest : ApiBase
	{
		public static async Task<ApiResponse<StringResponse>> PostAppAccessTokenRequest(StringRequest args) =>
			await PostRequestAsync<ApiResponse<StringResponse>>("/EditAccess/PostAppAccessToken", args);

		public static async Task<ApiResponse> PostAppVersionRequest(AppVersionRequest args) =>
			await PostRequestAsync<ApiResponse>("/EditConfig/PostAppVersion", args);

		public static async Task<ApiResponse> PostAppMaintenanceRequest(AppMaintenanceRequest args) =>
			await PostRequestAsync<ApiResponse>("/EditConfig/PostAppMaintenance", args);

		public static async Task<ApiResponse<StringArrayResponse>> PostTableFilesRequest(AppUploadFilesRequest args) =>
			await PostRequestAsync<ApiResponse<StringArrayResponse>>("/EditFiles/PostTableFiles", args);

		public static async Task<ApiResponse<StringArrayResponse>> PostBundleFilesRequest(AppUploadFilesRequest args) =>
			await PostRequestAsync<ApiResponse<StringArrayResponse>>("/EditFiles/PostBundleFiles", args);

		public static async Task<ApiResponse> PostUserNicknameRequest(StringRequest args) =>
			await PostRequestAsync<ApiResponse>("/User/PostUserNickname", args);

		public static async Task<ApiResponse> PostUserWithdrawalRequest(params object[] args) =>
			await PostRequestAsync<ApiResponse>("/User/PostUserWithdrawal", args);
	}
}
