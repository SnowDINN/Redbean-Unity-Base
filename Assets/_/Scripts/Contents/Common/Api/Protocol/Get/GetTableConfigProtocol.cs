using System.Threading.Tasks;

namespace Redbean.Api
{
	public class GetTableConfigProtocol : IApiContainer
	{
		public async Task<Response> Request(params object[] args)
		{
			return await ApiGetRequest.GetTableConfigRequest();
		}
	}
}