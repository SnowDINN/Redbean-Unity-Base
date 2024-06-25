using System.Threading.Tasks;

namespace Redbean.Api
{
	public class GetTableProtocol : IApiContainer
	{
		public async Task<Response> Request(params object[] args)
		{
			return await ApiGetRequest.GetTableRequest();
		}
	}
}