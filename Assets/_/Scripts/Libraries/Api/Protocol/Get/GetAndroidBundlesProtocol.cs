using System.Threading.Tasks;
using Redbean.Api;

namespace Redbean.Api
{
	public class GetAndroidBundlesProtocol : IApi
	{
		public Task Request(params object[] parameters)
		{
			return Task.CompletedTask;
		}
	}
}