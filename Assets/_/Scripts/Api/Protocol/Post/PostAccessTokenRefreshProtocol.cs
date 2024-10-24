using System.Threading;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class PostAccessTokenRefreshProtocol : ApiProtocol
	{
		protected override async Task<ApiResponse> Request(CancellationToken cancellationToken = default)
		{
			var response = await ApiPostRequest.PostAccessTokenRefreshRequest
					(new StringRequest(ApiToken.RefreshToken), cancellationToken);
			
			if (!response.IsSuccess)
				return response;

			ApiToken.SetAccessToken(response.Response);
			
			Log.Notice("Access token has been reissued.");
			return response;
		}
	}
}