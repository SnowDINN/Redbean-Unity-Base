using System.Threading;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class PostAppVersionProtocol : ApiProtocol
	{
		public override async Task<object> RequestAsync(CancellationToken cancellationToken = default)
		{
			return (await ApiPostRequest.PostAppVersionRequest(new AppVersionRequest
			{
				Type = (MobileType)args[0],
				Version = $"{args[1]}"
			}, cancellationToken)).Response;
		}
	}
}