using System;
using System.Threading;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class PostAppMaintenanceProtocol : ApiProtocol
	{
		public override async Task<object> RequestAsync(CancellationToken cancellationToken = default)
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