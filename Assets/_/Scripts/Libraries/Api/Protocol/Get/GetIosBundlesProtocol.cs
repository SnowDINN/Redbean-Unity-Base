﻿using System.Threading.Tasks;

namespace Redbean.Api
{
	public class GetIosBundlesProtocol : IApi
	{
		public async Task<Response> Request(params object[] args)
		{
			return await ApiGetRequest.GetiOSBundleFilesRequest(ApplicationSettings.Version);
		}
	}
}