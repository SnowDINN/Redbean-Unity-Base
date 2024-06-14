using System.Threading.Tasks;

namespace Redbean.Api
{
	public class GetAppConfigProtocol : IApi
	{
		public async Task<Response> Request(params object[] args)
		{
			return await ApiGetRequest.GetAppConfigRequest();
		}
	}
}