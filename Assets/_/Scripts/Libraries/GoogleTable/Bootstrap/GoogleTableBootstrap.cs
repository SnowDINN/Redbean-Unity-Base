using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Redbean.Api;
using Redbean.Container;

namespace Redbean.Table
{
	public class GoogleTableBootstrap
	{
		public static async Task Setup()
		{
			var request = await SingletonContainer.GetSingleton<ApiSingleton>().RequestApi<GetTableProtocol>();
			var response = request.ToConvert<Dictionary<string, string>>();
			if (!response.Any())
			{
				Log.Fail("Table", "Fail to load to the Google sheets.");
				return;
			}
			
			foreach (var table in response)
			{
				var tsv = table.Value.Split("\r\n");
				
				// Skip Name and Type Rows
				var skipRows = tsv.Skip(2);
				foreach (var item in skipRows)
				{
					var type = Type.GetType($"{nameof(Redbean)}.Table.{table.Key}");
					
					if (Activator.CreateInstance(type) is IGoogleTable instance)
						instance.Apply(item);
				}
			}
			
			Log.Success("Table", "Success to load to the tables.");
		}
	}
}