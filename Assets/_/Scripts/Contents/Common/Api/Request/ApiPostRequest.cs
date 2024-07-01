using System.Threading.Tasks;

namespace Redbean.Api
{
	public class ApiPostRequest : ApiBase
	{
		public static async Task<HttpResponse<AppVersionResponse>> PostAppVersionRequest(AppVersionRequest args) =>
			await SendPostRequest<HttpResponse<AppVersionResponse>>("/Config/PostAppVersion", args);

		public static async Task<HttpResponse<StringArrayResponse>> PostTableFilesRequest(AppUploadFilesRequest args) =>
			await SendPostRequest<HttpResponse<StringArrayResponse>>("/Storage/PostTableFiles", args);

		public static async Task<HttpResponse<StringArrayResponse>> PostBundleFilesRequest(AppUploadFilesRequest args) =>
			await SendPostRequest<HttpResponse<StringArrayResponse>>("/Storage/PostBundleFiles", args);

		public static async Task<HttpResponse<UserResponse>> PostUserNicknameRequest(params object[] args) =>
			await SendPostRequest<HttpResponse<UserResponse>>("/User/PostUserNickname?nickname={0}", args);
	}
}
