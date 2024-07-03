using System.Threading.Tasks;

namespace Redbean.Api
{
	public class PostUserWithdrawalProtocol : IApiContainer
	{
		public async Task<object> Request(params object[] args)
		{
			return (await ApiPostRequest.PostUserWithdrawalRequest()).Response;
		}
	}
}