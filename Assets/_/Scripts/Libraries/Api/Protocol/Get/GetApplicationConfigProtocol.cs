using System.Threading.Tasks;

namespace Redbean.Api
{
	public class GetApplicationConfigProtocol : IApi
	{
		public async Task<Response> Request(params object[] args)
		{
			return await ApiGetRequest.GetApplicationConfigRequest();
		}
	}
}