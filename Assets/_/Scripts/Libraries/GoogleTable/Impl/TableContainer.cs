using System;
using System.Linq;
using System.Threading.Tasks;
using Redbean.Api;

namespace Redbean.Table
{
	public partial class TableContainer : IAppBootstrap
	{
		public AppBootstrapType ExecutionType => AppBootstrapType.SignInUser;
		public int ExecutionOrder => 100;

		public async Task Setup()
		{
			var response = await this.RequestApi<GetTableProtocol>() as TableResponse;
			if (!response.Table.Any())
			{
				Log.Fail("Table", "Fail to load to the Google sheets.");
				return;
			}
			
			foreach (var table in response.Table)
			{
				var tsv = $"{table.Value}".Split("\r\n");
				
				// Skip Name and Type Rows
				var skipRows = tsv.Skip(2);
				foreach (var item in skipRows)
				{
					var type = Type.GetType($"{nameof(Redbean)}.Table.T{table.Key}");
					
					if (Activator.CreateInstance(type) is ITableContainer instance)
						instance.Apply(item);
				}
			}
			
			Log.Success("Table", "Success to load to the tables.");
		}

		public void Dispose()
		{
		}
	}
}