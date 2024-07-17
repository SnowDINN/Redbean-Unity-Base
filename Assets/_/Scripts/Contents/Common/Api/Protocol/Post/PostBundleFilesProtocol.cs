using System.Threading.Tasks;

namespace Redbean.Api
{
	public class PostBundleFilesProtocol : IApiContainer
	{
		public async Task<object> Request(params object[] args)
		{
			return (await ApiPostRequest.PostBundleFilesRequest(new AppUploadFilesRequest
			{
#if UNITY_ANDROID
				Type = MobileType.Android,
#elif UNITY_IOS
				Type = MobileType.iOS,
#else
				Type = MobileType.None,
#endif
				Files = args as RequestFile[]
			})).Response;
		}
	}
}