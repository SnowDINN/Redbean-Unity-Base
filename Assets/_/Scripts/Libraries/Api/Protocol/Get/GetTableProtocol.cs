using System.Threading.Tasks;

namespace Redbean.Api
{
	public class GetTableProtocol : IApi
	{
		public async Task<Response> Request(params object[] args)
		{
			return await ApiGetRequest.GetTableRequest(AppSettings.Version);
		}
	}
}