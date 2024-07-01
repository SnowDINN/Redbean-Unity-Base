﻿using System.Threading.Tasks;

namespace Redbean.Api
{
	public class GetRefreshAccessTokenProtocol : IApiContainer
	{
		public async Task<object> Request(params object[] args)
		{
			var request = await ApiGetRequest.GetRefreshAccessTokenRequest(ApiContainer.RefreshToken);
			if (request.ErrorCode > 0)
				return request.Response;

			ApiContainer.SetAccessToken(request.Response);
			
			Log.Notice("Access token has been reissued.");
			return request.Response;
		}
	}
}