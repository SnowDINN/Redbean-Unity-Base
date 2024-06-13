using System.Threading.Tasks;

namespace Redbean.Api
{
	public class DeleteAndroidBundleFilesProtocol : IApi
	{
		public async Task<Response> Request(params object[] args)
		{
			return await ApiDeleteRequest.DeleteAndroidBundleFileRequest(ApplicationSettings.Version);
		}
	}
}