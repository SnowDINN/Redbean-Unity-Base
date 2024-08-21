using System.Threading;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class GetAccessTokenRefreshProtocol : ApiProtocol
	{
		public override async Task<object> RequestAsync(CancellationToken cancellationToken = default)
		{
			var request = await ApiGetRequest.GetAccessTokenRefreshRequest(ApiAuthentication.RefreshToken);
			if (request.ErrorCode > 0)
				return request.Response;

			ApiAuthentication.SetAccessToken(request.Response);
			
			Log.Notice("Access token has been reissued.");
			return request.Response;
		}
	}
}