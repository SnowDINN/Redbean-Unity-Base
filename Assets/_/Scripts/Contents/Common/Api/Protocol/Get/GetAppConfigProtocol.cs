using System.Threading.Tasks;
using Redbean.MVP.Content;

namespace Redbean.Api
{
	public class GetAppConfigProtocol : IApiContainer
	{
		public async Task<object> Request(params object[] args)
		{
			var request = await ApiGetRequest.GetAppConfigRequest();
			new AppConfigModel(request.Response).ModelPublish();

			return request.Response;
		}
	}
}