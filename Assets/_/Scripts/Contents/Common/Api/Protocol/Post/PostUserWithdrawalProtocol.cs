﻿using System.Threading;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class PostUserWithdrawalProtocol : ApiProtocol
	{
		protected override async Task<object> Request(CancellationToken cancellationToken = default)
		{
			return (await ApiPostRequest.PostUserWithdrawalRequest(cancellationToken: cancellationToken)).Response;
		}
	}
}