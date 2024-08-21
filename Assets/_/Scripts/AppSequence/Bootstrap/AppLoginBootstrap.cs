using System.Threading.Tasks;
using Redbean.Api;

namespace Redbean
{
	public class AppLoginBootstrap : IAppBootstrap
	{
		public async Task Setup()
		{
			await this.RequestApi<GetTableProtocol>();
		}
		
		public void Dispose()
		{
			
		}
	}
}