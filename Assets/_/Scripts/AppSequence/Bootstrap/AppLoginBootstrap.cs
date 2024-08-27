using System.Threading.Tasks;
using Redbean.Api;

namespace Redbean
{
	public class AppLoginBootstrap : IAppBootstrap
	{
		public async Task Setup()
		{
			await this.GetProtocol<GetTableProtocol>().RequestAsync(AppLifeCycle.AppCancellationToken);
		}

		public Task Teardown()
		{
			return Task.CompletedTask;
		}
	}
}