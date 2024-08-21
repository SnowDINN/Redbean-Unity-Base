using System.Threading;
using System.Threading.Tasks;
using Redbean.MVP.Content;

namespace Redbean.Api
{
	public class GetAppConfigProtocol : ApiProtocol
	{
		public override async Task<object> RequestAsync(CancellationToken cancellationToken = default)
		{
			var request = await ApiGetRequest.GetAppConfigRequest();
			new AppConfigModel(request.Response).ModelPublish();

			return request.Response;
		}
	}
}