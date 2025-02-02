﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class EditAppMaintenanceProtocol : ApiProtocol
	{
		protected override async Task<ApiResponse> Request(CancellationToken cancellationToken = default)
		{
			return await ApiPostRequest.EditAppMaintenanceRequest(new AppMaintenanceRequest
			{
				Contents = $"{args[0]}",
				StartTime = (DateTime)args[1],
				EndTime = (DateTime)args[2]
			}, cancellationToken);
		}
	}
}