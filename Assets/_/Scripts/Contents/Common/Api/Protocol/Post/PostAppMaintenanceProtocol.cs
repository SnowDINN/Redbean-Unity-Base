using System;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class PostAppMaintenanceProtocol : IApiContainer
	{
		public async Task<object> Request(params object[] args)
		{
			return (await ApiPostRequest.PostAppMaintenanceRequest(new AppMaintenanceRequest
			{
				Contents = $"{args[0]}",
				StartTime = (DateTime)args[1],
				EndTime = (DateTime)args[2]
			})).Response;
		}
	}
}