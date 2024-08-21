using System.Threading;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class PostBundleFilesProtocol : ApiProtocol
	{
		public override async Task<object> RequestAsync(CancellationToken cancellationToken = default)
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