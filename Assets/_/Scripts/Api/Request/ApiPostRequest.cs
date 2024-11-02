using System.Threading;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiPostRequest : ApiBase
	{
		public static async Task<ApiResponse<UserAndTokenResponse>> PostAccessTokenAndUserRequest(UserRequest args, CancellationToken cancellationToken = default) =>
			await PostRequestAsync<ApiResponse<UserAndTokenResponse>>("/Authentication/PostAccessTokenAndUser", args, cancellationToken);

		public static async Task<ApiResponse<TokenResponse>> PostAccessTokenRefreshRequest(StringRequest args, CancellationToken cancellationToken = default) =>
			await PostRequestAsync<ApiResponse<TokenResponse>>("/Authentication/PostAccessTokenRefresh", args, cancellationToken);

		public static async Task<ApiResponse<EmptyResponse>> PostUserNicknameRequest(StringRequest args, CancellationToken cancellationToken = default) =>
			await PostRequestAsync<ApiResponse<EmptyResponse>>("/User/PostUserNickname", args, cancellationToken);

		public static async Task<ApiResponse<EmptyResponse>> PostUserWithdrawalRequest(UserWithdrawalRequest args, CancellationToken cancellationToken = default) =>
			await PostRequestAsync<ApiResponse<EmptyResponse>>("/User/PostUserWithdrawal", args, cancellationToken);

#if UNITY_EDITOR
		public static async Task<ApiResponse<StringResponse>> EditAppAccessTokenRequest(StringRequest args, CancellationToken cancellationToken = default) =>
			await PostRequestAsync<ApiResponse<StringResponse>>("/EditAccess/EditAppAccessToken", args, cancellationToken);

		public static async Task<ApiResponse<EmptyResponse>> EditAppVersionRequest(AppVersionRequest args, CancellationToken cancellationToken = default) =>
			await PostRequestAsync<ApiResponse<EmptyResponse>>("/EditConfig/EditAppVersion", args, cancellationToken);

		public static async Task<ApiResponse<EmptyResponse>> EditAppMaintenanceRequest(AppMaintenanceRequest args, CancellationToken cancellationToken = default) =>
			await PostRequestAsync<ApiResponse<EmptyResponse>>("/EditConfig/EditAppMaintenance", args, cancellationToken);

		public static async Task<ApiResponse<StringArrayResponse>> EditTableFilesRequest(AppUploadFilesRequest args, CancellationToken cancellationToken = default) =>
			await PostRequestAsync<ApiResponse<StringArrayResponse>>("/EditFiles/EditTableFiles", args, cancellationToken);

		public static async Task<ApiResponse<StringArrayResponse>> EditBundleFilesRequest(AppUploadFilesRequest args, CancellationToken cancellationToken = default) =>
			await PostRequestAsync<ApiResponse<StringArrayResponse>>("/EditFiles/EditBundleFiles", args, cancellationToken);
#endif
	}
}
