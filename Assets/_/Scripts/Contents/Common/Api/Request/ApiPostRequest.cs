using System.Threading;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiPostRequest : ApiBase
	{
		public static async Task<ApiResponse<StringResponse>> EditAppAccessTokenRequest(StringRequest args, CancellationToken cancellationToken = default) =>
			await PostRequestAsync<ApiResponse<StringResponse>>("/EditAccess/EditAppAccessToken", args, cancellationToken);

		public static async Task<ApiResponse> EditAppVersionRequest(AppVersionRequest args, CancellationToken cancellationToken = default) =>
			await PostRequestAsync<ApiResponse>("/EditConfig/EditAppVersion", args, cancellationToken);

		public static async Task<ApiResponse> EditAppMaintenanceRequest(AppMaintenanceRequest args, CancellationToken cancellationToken = default) =>
			await PostRequestAsync<ApiResponse>("/EditConfig/EditAppMaintenance", args, cancellationToken);

		public static async Task<ApiResponse<StringArrayResponse>> EditTableFilesRequest(AppUploadFilesRequest args, CancellationToken cancellationToken = default) =>
			await PostRequestAsync<ApiResponse<StringArrayResponse>>("/EditFiles/EditTableFiles", args, cancellationToken);

		public static async Task<ApiResponse<StringArrayResponse>> EditBundleFilesRequest(AppUploadFilesRequest args, CancellationToken cancellationToken = default) =>
			await PostRequestAsync<ApiResponse<StringArrayResponse>>("/EditFiles/EditBundleFiles", args, cancellationToken);

		public static async Task<ApiResponse> PostUserNicknameRequest(StringRequest args, CancellationToken cancellationToken = default) =>
			await PostRequestAsync<ApiResponse>("/User/PostUserNickname", args, cancellationToken);

		public static async Task<ApiResponse> PostUserWithdrawalRequest(object[] args = default, CancellationToken cancellationToken = default) =>
			await PostRequestAsync<ApiResponse>("/User/PostUserWithdrawal", args, cancellationToken);
	}
}
