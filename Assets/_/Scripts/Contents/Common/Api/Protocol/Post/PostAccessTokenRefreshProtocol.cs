using System.Threading;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class PostAccessTokenRefreshProtocol : ApiProtocol
	{
		protected override async Task<IApiResponse> Request(CancellationToken cancellationToken = default)
		{
			var request = await ApiPostRequest.PostAccessTokenRefreshRequest
					(new StringRequest(ApiAuthentication.RefreshToken), cancellationToken);
			
			if (request.ErrorCode != 0)
				return request.Response;

			ApiAuthentication.SetAccessToken(request.Response);
			
			Log.Notice("Access token has been reissued.");
			return request.Response;
		}
	}
}