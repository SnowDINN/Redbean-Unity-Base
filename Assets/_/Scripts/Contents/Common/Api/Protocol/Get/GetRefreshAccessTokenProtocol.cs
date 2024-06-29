using System.Threading.Tasks;

namespace Redbean.Api
{
	public class GetRefreshAccessTokenProtocol : IApiContainer
	{
		public async Task<object> Request(params object[] args)
		{
			var response = await ApiGetRequest.GetRefreshAccessTokenRequest(ApiContainer.RefreshToken);
			if (response.ErrorCode > 0)
				return response.Value;

			ApiContainer.SetAccessToken(response.Value);
			
			Log.Notice("Access token has been reissued.");
			return response.Value;
		}
	}
}