using System.Threading.Tasks;
using Redbean.Api;
using Redbean.Bundle;
using Redbean.Table;

namespace Redbean
{
	public class AppResourceBootstrap : IAppBootstrap
	{
		public async Task Setup()
		{
			await TableContainer.Setup();
			await BundleContainer.Setup();
		}

		public Task Teardown()
		{
			return Task.CompletedTask;
		}
	}
}