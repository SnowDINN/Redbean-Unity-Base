using System.Threading.Tasks;

namespace Redbean.Api
{
	public class GetTableConfigProtocol : IApiContainer
	{
		public async Task<object> Request(params object[] args)
		{
			return (await ApiGetRequest.GetTableConfigRequest()).Response;
		}
	}
}