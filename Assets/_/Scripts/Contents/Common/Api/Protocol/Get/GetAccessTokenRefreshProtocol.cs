﻿using System.Threading;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class GetAccessTokenRefreshProtocol : ApiProtocol
	{
		protected override async Task<object> Request(CancellationToken cancellationToken = default)
		{
			var request = await ApiGetRequest.GetAccessTokenRefreshRequest
					(new[] { ApiAuthentication.RefreshToken }, cancellationToken);
			
			if (request.ErrorCode != 0)
				return request.Response;

			ApiAuthentication.SetAccessToken(request.Response);
			
			Log.Notice("Access token has been reissued.");
			return request.Response;
		}
	}
}