using System.Threading.Tasks;
using Redbean.Bundle;
using Redbean.Table;

namespace Redbean
{
	public class OnLoginBootstrap : Bootstrap
	{
		protected override async Task Setup()
		{
			TableContainer.Setup();
			await BundleContainer.Setup();
		}
	}
}