using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Redbean.Api
{
	public class PostUserWithdrawalProtocol : ApiProtocol
	{
		protected override async Task<ApiResponse> Request(CancellationToken cancellationToken = default)
		{
			var response = await ApiPostRequest.PostUserWithdrawalRequest(new UserWithdrawalRequest
			{
				type = Enum.Parse<AuthenticationType>($"{args[0]}")
			}, cancellationToken);
			
			if (!response.IsSuccess)
				return response;
			
			PlayerPrefs.DeleteAll();

			return response;
		}
	}
}