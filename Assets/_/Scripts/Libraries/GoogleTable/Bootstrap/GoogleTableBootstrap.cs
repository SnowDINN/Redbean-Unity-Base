using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Redbean.Api;

namespace Redbean.Table
{
	public class GoogleTableBootstrap : IApplicationBootstrap
	{
		public int ExecutionOrder => 200;

		public async Task Setup()
		{
			var request = await ApiGetRequest.GetTableFilesRequest(ApplicationSettings.Version);
			var response = request.ToConvert<List<string>>();
			if (!response.Any())
			{
				Log.Fail("Table", "Fail to load to the Google sheets.");
				return;
			}
			
			foreach (var table in response)
			{
				var request = await this.RequestApi<GeTableProtocol>();
				var response = request.ToConvert<string[]>();
				
				var bytes = await request.GetBytesAsync(1024 * 1024 * 1024);
				var tsv = Encoding.UTF8.GetString(bytes).Split("\r\n");
				
				// Skip Name and Type Rows
				var skipRows = tsv.Skip(2);
				foreach (var item in skipRows)
				{
					var type = Type.GetType($"{nameof(Redbean)}.Table.{table}");
					
					if (Activator.CreateInstance(type) is IGoogleTable instance)
						instance.Apply(item);
				}
				
				Log.Notice($"{table} Table load is complete.");
			}
			
			Log.Success("Table", "Success to load to the tables.");
		}

		public void Dispose()
		{
			Log.System("Table has been terminated.");
		}
	}
}