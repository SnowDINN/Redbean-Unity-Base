using System.Threading.Tasks;

namespace Redbean.Api
{
	public class GetAppConfigProtocol : IApiContainer
	{
		public async Task<object> Request(params object[] args)
		{
			return (await ApiGetRequest.GetAppConfigRequest()).Response;
		}
	}
}