using System.Threading.Tasks;

namespace Redbean.Api
{
	public class GetIosBundlesProtocol : IApi
	{
		public Task Request(params object[] parameters)
		{
			return Task.CompletedTask;
		}
	}
}