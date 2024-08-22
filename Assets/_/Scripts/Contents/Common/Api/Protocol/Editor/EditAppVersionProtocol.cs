using System.Threading;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class EditAppVersionProtocol : ApiProtocol
	{
		public override async Task<object> RequestAsync(CancellationToken cancellationToken = default)
		{
			return (await ApiPostRequest.EditAppVersionRequest(new AppVersionRequest
			{
				Type = (MobileType)args[0],
				Version = $"{args[1]}"
			}, cancellationToken)).Response;
		}
	}
}