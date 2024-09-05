using System.Threading.Tasks;
using Redbean.Api;
using Redbean.Bundle;
using Redbean.Table;

namespace Redbean
{
	public class AppLoginBootstrap : IAppBootstrap
	{
		public async Task Setup()
		{
			await this.GetProtocol<GetTableProtocol>().RequestAsync(AppLifeCycle.AppCancellationToken);

			await TableContainer.Setup();
			await BundleContainer.Setup();
		}

		public Task Teardown()
		{
			return Task.CompletedTask;
		}
	}
}