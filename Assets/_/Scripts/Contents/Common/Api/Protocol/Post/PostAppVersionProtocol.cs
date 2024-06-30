using System.Threading.Tasks;

namespace Redbean.Api
{
	public class PostAppVersionProtocol : IApiContainer
	{
		public async Task<object> Request(params object[] args)
		{
			return (await ApiPostRequest.PostAppVersionRequest(new AppVersionRequest
			{
				Type = (MobileType)args[0],
				Version = $"{args[1]}"
			})).Value;
		}
	}
}