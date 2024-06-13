using System.Threading.Tasks;

namespace Redbean.Api
{
	public class DeleteTableFilesProtocol : IApi
	{
		public async Task<Response> Request(params object[] args)
		{
			return await ApiDeleteRequest.DeleteTableFilesRequest(ApplicationSettings.Version);
		}
	}
}