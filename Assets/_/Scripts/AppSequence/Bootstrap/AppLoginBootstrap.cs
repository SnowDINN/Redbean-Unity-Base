using System.Threading.Tasks;
using Redbean.Api;

namespace Redbean.Firebase
{
	public class AppLoginBootstrap : IAppBootstrap
	{
		public AppBootstrapType ExecutionType => AppBootstrapType.Login;
		public int ExecutionOrder => 100;
		
		public async Task Setup()
		{
			await this.RequestApi<GetTableProtocol>();
		}
		
		public void Dispose()
		{
			
		}
	}
}