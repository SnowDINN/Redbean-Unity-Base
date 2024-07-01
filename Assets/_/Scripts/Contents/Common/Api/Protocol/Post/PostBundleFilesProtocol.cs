using System.Threading.Tasks;

namespace Redbean.Api
{
	public class PostBundleFilesProtocol : IApiContainer
	{
		public async Task<object> Request(params object[] args)
		{
			return (await ApiPostRequest.PostBundleFilesRequest(new AppUploadFilesRequest
			{
				Type = AppSettings.PlatformType,
				Files = args as RequestFile[]
			})).Response;
		}
	}
}