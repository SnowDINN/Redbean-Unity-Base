using System.Threading.Tasks;

namespace Redbean.Api
{
	public class GetAccessTokenRefreshProtocol : IApiContainer
	{
		public async Task<object> Request(params object[] args)
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