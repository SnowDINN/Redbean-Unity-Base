using System.Threading.Tasks;

namespace Redbean.Api
{
	public class GetRefreshAccessTokenProtocol : IApiContainer
	{
		public async Task<Response> Request(params object[] args)
		{
			var request = await ApiGetRequest.GetRefreshAccessTokenRequest(ApiContainer.RefreshToken);
			if (request.Code > 0)
				return request;

			var tokenResponse = request.ToConvert<TokenResponse>();
			if (tokenResponse != null)
				ApiContainer.SetAccessToken(tokenResponse);
			
			Log.Notice("Access token has been reissued.");
			return request;
		}
	}
}