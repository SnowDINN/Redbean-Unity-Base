using System.Threading.Tasks;

namespace Redbean.Api
{
	public class GetTableFilesProtocol : IApi
	{
		public async Task<Response> Request(params object[] args)
		{
			return await ApiGetRequest.GetTableFilesRequest(AppSettings.Version);
		}
	}
}