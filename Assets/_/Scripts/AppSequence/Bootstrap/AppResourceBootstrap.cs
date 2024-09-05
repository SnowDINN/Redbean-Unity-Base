using System.Threading.Tasks;
using Redbean.Bundle;
using Redbean.Table;

namespace Redbean
{
	public class AppResourceBootstrap : Bootstrap
	{
		public override async Task Setup()
		{
			await base.Setup();
			
			TableContainer.Setup();
			await BundleContainer.Setup();
		}
	}
}