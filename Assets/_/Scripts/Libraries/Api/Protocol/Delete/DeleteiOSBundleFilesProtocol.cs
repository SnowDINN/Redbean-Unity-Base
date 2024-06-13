using System.Threading.Tasks;

namespace Redbean.Api
{
	public class DeleteiOSBundleFilesProtocol : IApi
	{
		public async Task<Response> Request(params object[] args)
		{
			return await ApiDeleteRequest.DeleteiOSBundleFilesRequest(ApplicationSettings.Version);
		}
	}
}