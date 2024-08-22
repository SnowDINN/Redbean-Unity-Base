using System.Threading;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiPostRequest : ApiBase
	{
		public static async Task<ApiResponse<StringResponse>> PostAppAccessTokenRequest(StringRequest args, CancellationToken cancellationToken = default) =>
			await PostRequestAsync<ApiResponse<StringResponse>>("/EditAccess/PostAppAccessToken", args, cancellationToken);

		public static async Task<ApiResponse> PostAppVersionRequest(AppVersionRequest args, CancellationToken cancellationToken = default) =>
			await PostRequestAsync<ApiResponse>("/EditConfig/PostAppVersion", args, cancellationToken);

		public static async Task<ApiResponse> PostAppMaintenanceRequest(AppMaintenanceRequest args, CancellationToken cancellationToken = default) =>
			await PostRequestAsync<ApiResponse>("/EditConfig/PostAppMaintenance", args, cancellationToken);

		public static async Task<ApiResponse<StringArrayResponse>> PostTableFilesRequest(AppUploadFilesRequest args, CancellationToken cancellationToken = default) =>
			await PostRequestAsync<ApiResponse<StringArrayResponse>>("/EditFiles/PostTableFiles", args, cancellationToken);

		public static async Task<ApiResponse<StringArrayResponse>> PostBundleFilesRequest(AppUploadFilesRequest args, CancellationToken cancellationToken = default) =>
			await PostRequestAsync<ApiResponse<StringArrayResponse>>("/EditFiles/PostBundleFiles", args, cancellationToken);

		public static async Task<ApiResponse> PostUserNicknameRequest(StringRequest args, CancellationToken cancellationToken = default) =>
			await PostRequestAsync<ApiResponse>("/User/PostUserNickname", args, cancellationToken);

		public static async Task<ApiResponse> PostUserWithdrawalRequest(object[] args = default, CancellationToken cancellationToken = default) =>
			await PostRequestAsync<ApiResponse>("/User/PostUserWithdrawal", args, cancellationToken);
	}
}
