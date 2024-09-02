using System.Threading;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class PostAccessTokenRefreshProtocol : ApiProtocol
	{
		protected override async Task<IApiResponse> Request(CancellationToken cancellationToken = default)
		{
			var response = await ApiPostRequest.PostAccessTokenRefreshRequest
					(new StringRequest(ApiAuthentication.RefreshToken), cancellationToken);
			
			if (response.ErrorCode != 0)
				return response;

			ApiAuthentication.SetAccessToken(response.Response);
			
			Log.Notice("Access token has been reissued.");
			return response;
		}
	}
}