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
				StartTime = DateTime.Parse($"{DateTime.Today.Year}-{DateTime.Today.Month}-{DateTime.Today.Day} {args[1]}:00"),
				EndTime = DateTime.Parse($"{DateTime.Today.Year}-{DateTime.Today.Month}-{DateTime.Today.Day} {args[2]}:00")
			})).Response;
		}
	}
}