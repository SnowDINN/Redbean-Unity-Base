﻿using System.Threading.Tasks;

namespace Redbean.Api
{
	public class GeTableProtocol : IApi
	{
		public async Task<Response> Request(params object[] args)
		{
			return await ApiGetRequest.GetTableFilesRequest(AppSettings.Version);
		}
	}
}