using System.Threading;
using System.Threading.Tasks;

namespace Redbean.Api
{
	public class PostUserWithdrawalProtocol : ApiProtocol
	{
		public override async Task<object> RequestAsync(CancellationToken cancellationToken = default)
		{
			return (await ApiPostRequest.PostUserWithdrawalRequest()).Response;
		}
	}
}